using ProjectedFileSystem.Core.Native;
using System.Runtime.InteropServices;

namespace ProjectedFileSystem.Core.Impl
{
  internal sealed partial class RunningInstance
  {
    private HRESULT ProcessPlaceholderRequest(
      PRJ_CALLBACK_DATA callbackData
    )
    {
      return _asyncOperations.ProcessCommandPossibleAsync(
        callbackData.NamespaceVirtualizationContext,
        callbackData.CommandId,
        cts => FileSystem.GetPlaceholder(
          callbackData.FilePathName,
          s => _outboundFunctions.PrjFileNameMatch(s, callbackData.FilePathName),
          cts.Token),
        placeholder =>
        {
          if (placeholder == null) return HRESULT.ERROR_FILE_NOT_FOUND;

          using (var buffer = LevelShifter.PRJ_PLACEHOLDER_INFOFromPlaceholderInfo(placeholder))
          {
            var result = _outboundFunctions.PrjWritePlaceholderInfo(callbackData.NamespaceVirtualizationContext, placeholder.DestinationFileName, buffer.Buffer, buffer.Sizes.totalSize);
            return result;
          }
        }
        );
    }
  }
}
