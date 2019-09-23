using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal class PRJ_NOTIFICATION_MAPPING
  {
    public PRJ_NOTIFY_TYPES NotificationBitMask;
    [MarshalAs(UnmanagedType.LPWStr)]
    public String NotificationRoot;
  }
}
