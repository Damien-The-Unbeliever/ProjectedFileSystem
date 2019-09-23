using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal class PRJ_FILE_BASIC_INFO
  {
    public bool IsDirectory;
    public long FileSize;
    public long CreationTime;
    public long LastAccessTime;
    public long LastWriteTime;
    public long ChangeTime;
    public int FileAttributes;
  }
}
