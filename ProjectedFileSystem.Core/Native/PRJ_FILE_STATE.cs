using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [Flags]
  internal enum PRJ_FILE_STATE
  {
    UnofficialNone = 0,
    PRJ_FILE_STATE_PLACEHOLDER = BoundConstants.PRJ_FILE_STATE.PRJ_FILE_STATE_PLACEHOLDER,
    PRJ_FILE_STATE_HYDRATED_PLACEHOLDER = BoundConstants.PRJ_FILE_STATE.PRJ_FILE_STATE_HYDRATED_PLACEHOLDER,
    PRJ_FILE_STATE_DIRTY_PLACEHOLDER = BoundConstants.PRJ_FILE_STATE.PRJ_FILE_STATE_DIRTY_PLACEHOLDER,
    PRJ_FILE_STATE_FULL = BoundConstants.PRJ_FILE_STATE.PRJ_FILE_STATE_FULL,
    PRJ_FILE_STATE_TOMBSTONE = BoundConstants.PRJ_FILE_STATE.PRJ_FILE_STATE_TOMBSTONE,
  }
}
