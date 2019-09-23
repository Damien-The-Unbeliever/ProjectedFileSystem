using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal class PRJ_STARTVIRTUALIZING_OPTIONS
  {
    public PRJ_STARTVIRTUALIZING_FLAGS Flags;
    public int PoolThreadCount;
    public int ConcurrentThreadCount;
    public PRJ_NOTIFICATION_MAPPING[] NotificationMappings;
    public int NotificationMappingsCount;
  }
}
