using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  internal enum PRJ_STARTVIRTUALIZING_FLAGS
  {
    PRJ_FLAG_NONE = BoundConstants.PRJ_STARTVIRTUALIZING_FLAGS.PRJ_FLAG_NONE,
    PRJ_FLAG_USE_NEGATIVE_PATH_CACHE = BoundConstants.PRJ_STARTVIRTUALIZING_FLAGS.PRJ_FLAG_USE_NEGATIVE_PATH_CACHE
  }
}
