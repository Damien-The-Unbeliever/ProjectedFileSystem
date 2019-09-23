using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal struct PRJ_VIRTUALIZATION_INSTANCE_INFO
  {
    public Guid InstanceID;
    public int WriteAlignment;
  }
}
