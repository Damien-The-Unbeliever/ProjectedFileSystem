using ProjectedFileSystem.Core.Native;

namespace ProjectedFileSystem.Core.Impl
{
  internal sealed partial class RunningInstance
  {
    internal HRESULT ProcessDataRequest(
      PRJ_CALLBACK_DATA callbackData,
      ulong byteOffset,
      uint length
    )
    {
      var weirdnessOffset = byteOffset % (ulong)_virtualizationInfo.WriteAlignment;
      byteOffset -= weirdnessOffset;
      length += (uint)weirdnessOffset;
      var stream = new FileWriteStream(byteOffset,
        _virtualizationInfo.WriteAlignment,
        callbackData.NamespaceVirtualizationContext,
        callbackData.DataStreamId, _outboundFunctions);

      return _asyncOperations.ProcessCommandPossibleAsync(
        callbackData.NamespaceVirtualizationContext,
        callbackData.CommandId,
        cts => FileSystem.GetFileData(
          callbackData.FilePathName,
          LevelShifter.PlaceholderVersionFromPRJ_PLACEHOLDER_VERSION_INFO(callbackData.VersionInfo),
          (long)byteOffset,
          (int)length,
          stream).ContinueWith(t=> {
            stream.Close();
          }));
    }
  }
}
