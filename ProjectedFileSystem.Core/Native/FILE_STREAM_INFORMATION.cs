using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal class FILE_STREAM_INFORMATION
  {
    public uint NextEntryOffset;
    public uint StreamNameLength;
    public long StreamSize;
    public long StreamAllocationSize;
    /* Stream name in UTF16 characters */
  }
}
