﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  [Flags]
  internal enum PRJ_NOTIFY_TYPES
  {
    PRJ_NOTIFY_NONE = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_NONE,
    PRJ_NOTIFY_SUPPRESS_NOTIFICATIONS = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_SUPPRESS_NOTIFICATIONS,
    PRJ_NOTIFY_FILE_OPENED = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_OPENED,
    PRJ_NOTIFY_NEW_FILE_CREATED = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_NEW_FILE_CREATED,
    PRJ_NOTIFY_FILE_OVERWRITTEN = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_OVERWRITTEN,
    PRJ_NOTIFY_PRE_DELETE = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_PRE_DELETE,
    PRJ_NOTIFY_PRE_RENAME = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_PRE_RENAME,
    PRJ_NOTIFY_PRE_SET_HARDLINK = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_PRE_SET_HARDLINK,
    PRJ_NOTIFY_FILE_RENAMED = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_RENAMED,
    PRJ_NOTIFY_HARDLINK_CREATED = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_HARDLINK_CREATED,
    PRJ_NOTIFY_FILE_HANDLE_CLOSED_NO_MODIFICATION = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_HANDLE_CLOSED_NO_MODIFICATION,
    PRJ_NOTIFY_FILE_HANDLE_CLOSED_FILE_MODIFIED = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_HANDLE_CLOSED_FILE_MODIFIED,
    PRJ_NOTIFY_FILE_HANDLE_CLOSED_FILE_DELETED = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_HANDLE_CLOSED_FILE_DELETED,
    PRJ_NOTIFY_FILE_PRE_CONVERT_TO_FULL = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_PRE_CONVERT_TO_FULL,
    PRJ_NOTIFY_USE_EXISTING_MASK = BoundConstants.PRJ_NOTIFY_TYPES.PRJ_NOTIFY_USE_EXISTING_MASK
  }
}
