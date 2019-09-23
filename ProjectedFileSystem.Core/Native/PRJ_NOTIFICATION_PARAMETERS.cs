using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [StructLayout(LayoutKind.Explicit)]
  internal class PRJ_NOTIFICATION_PARAMETERS
  {
    [FieldOffset(0)]
    public PRJ_NOTIFY_TYPES NotificationMask;
    [FieldOffset(0)]
    public bool IsFileModified;

  }
}
