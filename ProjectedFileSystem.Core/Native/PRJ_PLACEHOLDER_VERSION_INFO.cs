using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal class PRJ_PLACEHOLDER_VERSION_INFO
  {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.PRJ_PLACEHOLDER_ID_LENGTH)]
    public byte[] ProviderID;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.PRJ_PLACEHOLDER_ID_LENGTH)]
    public byte[] ContentID;
  }
}
