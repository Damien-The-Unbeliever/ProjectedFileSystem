using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal struct PRJ_COMPLETE_COMMAND_EXTENDED_PARAMETERS
  {
    internal PRJ_COMPLETE_COMMAND_TYPE CommandType;
    [StructLayout(LayoutKind.Explicit)]
    internal struct Union
    {
      [FieldOffset(0)]
      internal PRJ_NOTIFY_TYPES NotificationMask;
      [FieldOffset(0)]
      internal IntPtr DirEntryBufferHandle;
    }
    internal Union _Union;
  }
}
 