using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal struct PRJ_PLACEHOLDER_INFO
  {
    public PRJ_FILE_BASIC_INFO FileBasicInfo;

    [StructLayout(LayoutKind.Sequential)]
    internal struct _EaInformation
    {
      public int EaBufferSize;
      public int OffsetToFirstEa;
    }
    public _EaInformation EaInformation;

    [StructLayout(LayoutKind.Sequential)]
    internal struct _SecurityInformation
    {
      public int SecurityBufferSize;
      public int OffsetToSecurityDescriptor;
    }
    public _SecurityInformation SecurityInformation;

    [StructLayout(LayoutKind.Sequential)]
    internal struct _StreamsInformation
    {
      public int StreamsInfoBufferSize;
      public int OffsetToFirstStreamInfo;
    }
    public _StreamsInformation StreamsInformation;

    public PRJ_PLACEHOLDER_VERSION_INFO VersionInfo;

    /* Start optional data */
  }

}
