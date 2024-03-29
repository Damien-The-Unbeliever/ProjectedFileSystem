﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  internal enum PRJ_NOTIFICATION
  {
    PRJ_NOTIFICATION_FILE_OPENED = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_OPENED,
    PRJ_NOTIFICATION_NEW_FILE_CREATED = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_NEW_FILE_CREATED,
    PRJ_NOTIFICATION_FILE_OVERWRITTEN = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_OVERWRITTEN,
    PRJ_NOTIFICATION_PRE_DELETE = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_PRE_DELETE,
    PRJ_NOTIFICATION_PRE_RENAME = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_PRE_RENAME,
    PRJ_NOTIFICATION_PRE_SET_HARDLINK = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_PRE_SET_HARDLINK,
    PRJ_NOTIFICATION_FILE_RENAMED = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_RENAMED,
    PRJ_NOTIFICATION_HARDLINK_CREATED = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_HARDLINK_CREATED,
    PRJ_NOTIFICATION_FILE_HANDLE_CLOSED_NO_MODIFICATION = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_HANDLE_CLOSED_NO_MODIFICATION,
    PRJ_NOTIFICATION_FILE_HANDLE_CLOSED_FILE_MODIFIED = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_HANDLE_CLOSED_FILE_MODIFIED,
    PRJ_NOTIFICATION_FILE_HANDLE_CLOSED_FILE_DELETED = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_HANDLE_CLOSED_FILE_DELETED,
    PRJ_NOTIFICATION_FILE_PRE_CONVERT_TO_FULL = BoundConstants.PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_PRE_CONVERT_TO_FULL,
  }
}
