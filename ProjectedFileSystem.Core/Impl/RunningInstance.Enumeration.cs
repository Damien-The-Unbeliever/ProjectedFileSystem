using ProjectedFileSystem.Core.FileSystem;
using ProjectedFileSystem.Core.Native;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Impl
{
  internal sealed partial class RunningInstance
  {
    private class OngoingEnumeration
    {
      public IAsyncEnumerator<BasicInfo> Enumerator;
      public string Path;
      public PlaceholderVersion VersionInfo;
      public CancellationTokenSource CancellationForEnumeration;
      public string MatchPattern;
      public EnumerationState State;
    }
    private enum EnumerationState
    {
      Init = 0,
      CurrentPending = 1,
      Ongoing = 2,
      NotOneSent = 3
    }
    private readonly ConcurrentDictionary<Guid, OngoingEnumeration> _inFlightEnumerations
      = new ConcurrentDictionary<Guid, OngoingEnumeration>();

    private HRESULT ProcessStartEnumeration(
      PRJ_CALLBACK_DATA callbackData,
      Guid enumerationId
    )
    {
      try
      {
        var cts = new CancellationTokenSource();
        var ongoing = new OngoingEnumeration
        {
          Path = callbackData.FilePathName,
          VersionInfo = LevelShifter.PlaceholderVersionFromPRJ_PLACEHOLDER_VERSION_INFO(callbackData.VersionInfo),
          State = EnumerationState.Init
        };
        if (!_inFlightEnumerations.TryAdd(enumerationId, ongoing))
        {
          cts.Cancel();
          return HRESULT.E_INVALIDARG;
        }
        return HRESULT.S_OK;
      }
      catch(Exception){
        return HRESULT.E_INVALIDARG;
      }
    }

    private HRESULT ProcessEndEnumeration(
      PRJ_CALLBACK_DATA callbackData,
      Guid enumerationId
    )
    {
      if (!_inFlightEnumerations.TryRemove(enumerationId, out var ongoing)) return HRESULT.E_INVALIDARG;
      return _asyncOperations.ProcessCommandPossibleAsync(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts =>
      {
        ongoing.CancellationForEnumeration.Cancel();
        return ongoing.Enumerator.DisposeAsync();
      });
    }

    private HRESULT ProcessEnumerationStep(
      PRJ_CALLBACK_DATA callbackData,
      Guid enumerationId,
      String searchExpression,
      IntPtr dirEntryBufferHandle
    )
    {
      if (!_inFlightEnumerations.TryGetValue(enumerationId, out var ongoing)) return HRESULT.E_INVALIDARG;
      InitializeEnumeration(callbackData, ongoing);
      if (ongoing.State == EnumerationState.CurrentPending)
      {
        if (!SendCurrentEnumeratorValue(ongoing, dirEntryBufferHandle)) return HRESULT.ERROR_INSUFFICIENT_BUFFER;
        ongoing.State = EnumerationState.Ongoing;
      }
      else
      {
        ongoing.State = EnumerationState.NotOneSent;
      }
      var mnTask = ongoing.Enumerator.MoveNextAsync();

      while (true)
      {
        if (mnTask.IsCompleted)
        {
          if (!mnTask.Result) return HRESULT.S_OK;
          if (SendCurrentEnumeratorValue(ongoing, dirEntryBufferHandle))
          {
            ongoing.State = EnumerationState.Ongoing;
            mnTask = ongoing.Enumerator.MoveNextAsync();
            continue;
          }
          else
          {
            var returnSync = ongoing.State == EnumerationState.NotOneSent ?
              HRESULT.ERROR_INSUFFICIENT_BUFFER :
              HRESULT.S_OK;
            ongoing.State = EnumerationState.CurrentPending;
            return returnSync;
          }
        }
        var cts = _asyncOperations.GetCancellationTokenForCommand(callbackData.CommandId);
        cts.Token.Register(ongoing.CancellationForEnumeration.Cancel);
        mnTask.AsTask().ContinueWith(answer => ContinueEnumerationAsync(callbackData.NamespaceVirtualizationContext, answer.Result, ongoing, dirEntryBufferHandle, callbackData.CommandId), TaskContinuationOptions.RunContinuationsAsynchronously);
        return HRESULT.ERROR_IO_PENDING;
      }
    }

    private void ContinueEnumerationAsync(IntPtr namespaceVirtualizationContext, bool enumerationResult, OngoingEnumeration enumeration, IntPtr dirEntryBufferHandle, int commandId)
    {
      while (true)
      {
        if (!enumerationResult)
        {
          _asyncOperations.AsyncProvideHRESULTWithDirEntry(namespaceVirtualizationContext, commandId, HRESULT.S_OK, dirEntryBufferHandle);
          return;
        }
        if (SendCurrentEnumeratorValue(enumeration, dirEntryBufferHandle))
        {
          enumeration.State = EnumerationState.Ongoing;
          var newTask = enumeration.Enumerator.MoveNextAsync();
          if (newTask.IsCompleted)
          {
            enumerationResult = newTask.Result;
            continue;
          }
          else
          {
              newTask.AsTask().ContinueWith(answer => ContinueEnumerationAsync(namespaceVirtualizationContext, answer.Result, enumeration, dirEntryBufferHandle, commandId), TaskContinuationOptions.RunContinuationsAsynchronously);
            return;
          }
        }
        else
        {
          var returnSync = enumeration.State == EnumerationState.NotOneSent ?
            HRESULT.ERROR_INSUFFICIENT_BUFFER :
            HRESULT.S_OK;
          enumeration.State = EnumerationState.CurrentPending;
          _asyncOperations.AsyncProvideHRESULTWithDirEntry(namespaceVirtualizationContext, commandId, returnSync, dirEntryBufferHandle);
          return;
        }
      }
    }

    private bool SendCurrentEnumeratorValue(OngoingEnumeration ongoing, IntPtr dirEntryBufferHandle)
    {

      var current = ongoing.Enumerator.Current;
      if (_outboundFunctions.PrjFillDirEntryBuffer(
        current.Name,
        LevelShifter.PRJ_FILE_BASIC_INFOFromBasicInfo(current),
        dirEntryBufferHandle) == HRESULT.S_OK)
      {
        return true;
      }
      return false;
    }
    private void InitializeEnumeration(PRJ_CALLBACK_DATA callbackData, OngoingEnumeration ongoing)
    {
      if ((callbackData.Flags & PRJ_CALLBACK_DATA_FLAGS.PRJ_CB_DATA_FLAG_ENUM_RESTART_SCAN) != 0 || ongoing.State == EnumerationState.Init)
      {
        ongoing.MatchPattern = callbackData.FilePathName;
        if (ongoing.CancellationForEnumeration != null)
        {
          ongoing.CancellationForEnumeration.Cancel();
        }
        ongoing.CancellationForEnumeration = new CancellationTokenSource();
        ongoing.Enumerator = FileSystem.FindContent(
          ongoing.Path,
          ongoing.VersionInfo,
          s => _outboundFunctions.PrjFileNameMatch(s, ongoing.MatchPattern),
          new SortComparer(_outboundFunctions),
          ongoing.CancellationForEnumeration.Token
          );
        ongoing.State = EnumerationState.Ongoing;
      }
    }

    private sealed class SortComparer : IComparer<string>
    {
      private readonly Functions _outboundFunctions;
      public SortComparer(Functions outboundFunctions)
      {
        _outboundFunctions = outboundFunctions;
      }
      public int Compare(string x, string y)
      {
        return _outboundFunctions.PrjFileNameCompare(x, y);
      }
    }
  }
}
