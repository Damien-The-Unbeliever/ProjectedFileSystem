using ProjectedFileSystem.Core.FileSystem;
using ProjectedFileSystem.Core.Native;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Impl
{
  internal sealed class AsyncManager : IDisposable
  {
    private readonly ConcurrentDictionary<int, CancellationTokenSource> _inFlight
  = new ConcurrentDictionary<int, CancellationTokenSource>();

    private readonly Functions _outboundFunctions;
    public AsyncManager(Functions outboundFunctions)
    {
      _outboundFunctions = outboundFunctions;
    }
    public CancellationTokenSource GetCancellationTokenForCommand(int commandId)
    {
      return _inFlight.GetOrAdd(commandId, (key) => new CancellationTokenSource());
    }
    public void ProcessCancelCommand(int commandId)
    {
      if (_inFlight.TryRemove(commandId, out var cts))
      {
        cts.Cancel();
      }
    }

    public HRESULT ProcessCommandPossibleAsync<TIntermediate>(
      IntPtr namespaceVirtualizationContext,
      int commandId,
      Func<CancellationTokenSource, Task<TIntermediate>> kickOff,
      Func<TIntermediate, HRESULT> processFinal)
    {
      try
      {
        var cts = new CancellationTokenSource();
        if (!_inFlight.TryAdd(commandId, cts)) return HRESULT.E_INVALIDARG;
        var inter = kickOff(cts);
        if (inter.IsCompletedSuccessfully)
        {
          _inFlight.TryRemove(commandId, out _);
          return processFinal(inter.Result);
        }
        if (inter.IsFaulted)
        {
          return HRESULT.E_INVALIDARG;
        }
        inter.ContinueWith(inter2 =>
        {
          try
          {
            var final = processFinal(inter2.Result);
            AsyncProvideHRESULT(namespaceVirtualizationContext, commandId, final);
          }
          catch(Exception)
          {
            AsyncProvideHRESULT(namespaceVirtualizationContext, commandId, HRESULT.E_INVALIDARG);
          }
        }, TaskContinuationOptions.OnlyOnRanToCompletion);
        inter.ContinueWith(inter3 =>
        {
          AsyncProvideHRESULT(namespaceVirtualizationContext, commandId, HRESULT.E_INVALIDARG);
        }, TaskContinuationOptions.OnlyOnFaulted);
        return HRESULT.ERROR_IO_PENDING;
      }
      catch (Exception)
      {
        return HRESULT.E_INVALIDARG;
      }
    }

    public HRESULT ProcessCommandPossibleAsyncWithNotificationMask(
      IntPtr namespaceVirtualizationContext,
      int commandId,
      Func<CancellationTokenSource, Task<NotificationRequired>> kickOff,
      PRJ_NOTIFICATION_PARAMETERS parameters
    )
    {
      try
      {
        var cts = new CancellationTokenSource();
        if (!_inFlight.TryAdd(commandId, cts)) return HRESULT.E_INVALIDARG;
        var inter = kickOff(cts);
        if (inter.IsCompletedSuccessfully)
        {
          _inFlight.TryRemove(commandId, out _);
          parameters.NotificationMask = (PRJ_NOTIFY_TYPES)(int)inter.Result;
          return HRESULT.S_OK;
        }
        if (inter.IsFaulted)
        {
          return HRESULT.E_INVALIDARG;
        }
        inter.ContinueWith(inter2 =>
        {
          AsyncProvideHRESULTWithNotificationMask(namespaceVirtualizationContext, commandId, HRESULT.S_OK, (PRJ_NOTIFY_TYPES)(int)inter2.Result);
        }, TaskContinuationOptions.OnlyOnRanToCompletion);
        inter.ContinueWith(inter3 =>
        {
          AsyncProvideHRESULT(namespaceVirtualizationContext, commandId, HRESULT.E_INVALIDARG);
        }, TaskContinuationOptions.OnlyOnFaulted);
        return HRESULT.ERROR_IO_PENDING;
      }
      catch (Exception)
      {
        return HRESULT.E_INVALIDARG;
      }
    }
    public HRESULT ProcessCommandPossibleAsync(
      IntPtr namespaceVirtualizationContext,
     int commandId,
     Func<CancellationTokenSource, Task> kickOff
   )
    {
      try
      {
        var cts = new CancellationTokenSource();
        if (!_inFlight.TryAdd(commandId, cts)) return HRESULT.E_INVALIDARG;
        var inter = kickOff(cts);
        if (inter.IsCompletedSuccessfully)
        {
          _inFlight.TryRemove(commandId, out _);
          return HRESULT.S_OK;
        }
        if (inter.IsFaulted)
        {
          return HRESULT.E_INVALIDARG;
        }
        inter.ContinueWith(inter2 =>
        {
          AsyncProvideHRESULT(namespaceVirtualizationContext, commandId, HRESULT.S_OK);
        },TaskContinuationOptions.OnlyOnRanToCompletion);
        inter.ContinueWith(inter3 =>
        {
          AsyncProvideHRESULT(namespaceVirtualizationContext, commandId, HRESULT.E_INVALIDARG);
        }, TaskContinuationOptions.OnlyOnFaulted);
        return HRESULT.ERROR_IO_PENDING;
      }
      catch (Exception)
      {
        return HRESULT.E_INVALIDARG;
      }
    }
    public HRESULT ProcessCommandPossibleAsync(
      IntPtr namespaceVirtualizationContext,
      int commandId,
      Func<CancellationTokenSource, ValueTask> kickOff
    )
    {
      try
      {
        var cts = new CancellationTokenSource();
        if (!_inFlight.TryAdd(commandId, cts)) return HRESULT.E_INVALIDARG;
        var inter = kickOff(cts);
        if (inter.IsCompletedSuccessfully)
        {
          _inFlight.TryRemove(commandId, out _);
          return HRESULT.S_OK;
        }
        Task task = inter.AsTask();
        task.ContinueWith(_ =>
        {
          AsyncProvideHRESULT(namespaceVirtualizationContext, commandId, HRESULT.S_OK);
        }, TaskContinuationOptions.OnlyOnRanToCompletion);
        task.ContinueWith(_ =>
        {
          AsyncProvideHRESULT(namespaceVirtualizationContext, commandId, HRESULT.E_INVALIDARG);
        }, TaskContinuationOptions.OnlyOnFaulted);

        return HRESULT.ERROR_IO_PENDING;
      }
      catch (Exception)
      {
        return HRESULT.E_INVALIDARG;
      }
    }

    private void AsyncProvideHRESULT(IntPtr namespaceVirtualizationContext, int commandId, HRESULT final)
    {
      _inFlight.TryRemove(commandId, out _);
      _outboundFunctions.PrjCompleteCommand(namespaceVirtualizationContext, commandId, final, IntPtr.Zero);
    }

    public void AsyncProvideHRESULTWithDirEntry(IntPtr namespaceVirtualizationContext, int commandId, HRESULT final, IntPtr dirEntryBufferHandle)
    {
      var args = new PRJ_COMPLETE_COMMAND_EXTENDED_PARAMETERS
      {
        CommandType = PRJ_COMPLETE_COMMAND_TYPE.PRJ_COMPLETE_COMMAND_TYPE_ENUMERATION
      };
      args._Union.DirEntryBufferHandle = dirEntryBufferHandle;
      _inFlight.TryRemove(commandId, out _);
      using (var buffer = new NativeBuffer<PRJ_COMPLETE_COMMAND_EXTENDED_PARAMETERS>(args))
      {
        _outboundFunctions.PrjCompleteCommand(namespaceVirtualizationContext, commandId, final, buffer.Buffer);
      }
    }

    private void AsyncProvideHRESULTWithNotificationMask(IntPtr namespaceVirtualizationContext, int commandId, HRESULT final, PRJ_NOTIFY_TYPES notifyTypes)
    {
      var args = new PRJ_COMPLETE_COMMAND_EXTENDED_PARAMETERS
      {
        CommandType = PRJ_COMPLETE_COMMAND_TYPE.PRJ_COMPLETE_COMMAND_TYPE_ENUMERATION
      };
      args._Union.NotificationMask = notifyTypes;
      _inFlight.TryRemove(commandId, out _);
      using (var buffer = new NativeBuffer<PRJ_COMPLETE_COMMAND_EXTENDED_PARAMETERS>(args))
      {
        _outboundFunctions.PrjCompleteCommand(namespaceVirtualizationContext, commandId, final, buffer.Buffer);
      }
    }

    public void Dispose()
    {
      foreach(var cts in _inFlight.Values)
      {
        cts.Cancel();
      }
    }
  }
}
