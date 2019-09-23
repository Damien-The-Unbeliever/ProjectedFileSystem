using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [Flags]
  internal enum  PRJ_CALLBACK_DATA_FLAGS
  {
    UnofficialNone = 0,
    PRJ_CB_DATA_FLAG_ENUM_RESTART_SCAN = BoundConstants.PRJ_CALLBACK_DATA_FLAGS.PRJ_CB_DATA_FLAG_ENUM_RESTART_SCAN,
		PRJ_CB_DATA_FLAG_ENUM_RETURN_SINGLE_ENTRY = BoundConstants.PRJ_CALLBACK_DATA_FLAGS.PRJ_CB_DATA_FLAG_ENUM_RETURN_SINGLE_ENTRY
  }
}
