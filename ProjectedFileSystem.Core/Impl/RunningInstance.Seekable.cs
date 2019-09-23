using ProjectedFileSystem.Core.Interfaces;
using ProjectedFileSystem.Core.Native;

namespace ProjectedFileSystem.Core.Impl
{
  internal sealed partial class RunningInstance
  {
    private ISeekableFileSystem Seekable => FileSystem as ISeekableFileSystem;

    private HRESULT ProcessQueryFileName(
      PRJ_CALLBACK_DATA callbackData
    )
    {
      return _asyncOperations.ProcessCommandPossibleAsync(
        callbackData.NamespaceVirtualizationContext,
        callbackData.CommandId,
        cts =>
        {
          return Seekable.FileExists(
            callbackData.FilePathName,
            fileName => _outboundFunctions.PrjFileNameCompare(fileName, callbackData.FilePathName)==0,
            cts.Token);
        },
        answer => answer ? HRESULT.S_OK : HRESULT.ERROR_FILE_NOT_FOUND
        );
    }
  }
}
