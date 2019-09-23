using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal class PRJ_CALLBACK_DATA
  {
    public int Size;
    public PRJ_CALLBACK_DATA_FLAGS Flags;
    public IntPtr NamespaceVirtualizationContext;
    public int CommandId;
    public Guid FileId;
    public Guid DataStreamId;
    [MarshalAs(UnmanagedType.LPWStr)]
    public String FilePathName;
    public PRJ_PLACEHOLDER_VERSION_INFO VersionInfo;
    public int TriggeringProcessId;
    [MarshalAs(UnmanagedType.LPWStr)]
    public String TriggeringProcessImageFileName;
    public IntPtr InstanceContext;
  }
}
