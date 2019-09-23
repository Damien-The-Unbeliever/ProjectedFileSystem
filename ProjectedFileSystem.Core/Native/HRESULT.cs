using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  internal enum HRESULT : uint
  {
    S_OK = 0,
    S_FALSE = 1,
    ERROR_REPARSE_TAG_MISMATCH = 2147946794,
    ERROR_FILE_NOT_FOUND = 2147942402,
    ERROR_IO_PENDING = 2147943397,
    ERROR_INSUFFICIENT_BUFFER = 2147942522,
    E_INVALIDARG = 0x80070057
  }
}
