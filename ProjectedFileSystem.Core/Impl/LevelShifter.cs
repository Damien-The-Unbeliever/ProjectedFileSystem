using ProjectedFileSystem.Core.FileSystem;
using ProjectedFileSystem.Core.Native;
using System.Runtime.InteropServices;

namespace ProjectedFileSystem.Core.Impl
{
  internal static class LevelShifter
  {
    internal static PlaceholderVersion PlaceholderVersionFromPRJ_PLACEHOLDER_VERSION_INFO(PRJ_PLACEHOLDER_VERSION_INFO original)
    {
      return new PlaceholderVersion(original.ProviderID, original.ContentID);
    }
    internal static PRJ_PLACEHOLDER_VERSION_INFO PRJ_PLACEHOLDER_VERSION_INFOFromPlaceholderVersion(PlaceholderVersion original)
    {
      var pvi = new PRJ_PLACEHOLDER_VERSION_INFO
      {
        ContentID = new byte[Constants.PRJ_PLACEHOLDER_ID_LENGTH],
        ProviderID = new byte[Constants.PRJ_PLACEHOLDER_ID_LENGTH]
      };
      original.ContentId.CopyTo(pvi.ContentID);
      original.ProviderId.CopyTo(pvi.ProviderID);
      return pvi;
    }
    internal static NativeBuffer<PRJ_PLACEHOLDER_INFO> PRJ_PLACEHOLDER_INFOFromPlaceholderInfo(PlaceholderInfo original)
    {
      var ph = new PRJ_PLACEHOLDER_INFO
      {
        FileBasicInfo = PRJ_FILE_BASIC_INFOFromBasicInfo(original.BasicInfo),
        VersionInfo = PRJ_PLACEHOLDER_VERSION_INFOFromPlaceholderVersion(original.Version)
      };
      if (original.Security == null) return new NativeBuffer<PRJ_PLACEHOLDER_INFO>(ph);
      var securityBuffer = original.Security.GetSecurityDescriptorBinaryForm();
      ph.SecurityInformation.OffsetToSecurityDescriptor = Marshal.SizeOf<PRJ_PLACEHOLDER_INFO>();
      ph.SecurityInformation.SecurityBufferSize = securityBuffer.Length;
      return new NativeBuffer<PRJ_PLACEHOLDER_INFO>(ph, securityBuffer);
      
    }

    internal static PRJ_FILE_BASIC_INFO PRJ_FILE_BASIC_INFOFromBasicInfo(BasicInfo original)
    {
      return new PRJ_FILE_BASIC_INFO
      {
        IsDirectory = original.IsDirectory,
        FileSize = original.Size,
        ChangeTime = original.Changed?.ToFileTime()??0,
        CreationTime = original.Created?.ToFileTime()??0,
        LastAccessTime = original.Accessed?.ToFileTime()??0,
        LastWriteTime = original.Written?.ToFileTime()??0,
        FileAttributes = (int)original.Attributes
      };
    }
  }
}
