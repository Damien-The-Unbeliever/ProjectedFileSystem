using ProjectedFileSystem.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.FileSystem
{
  /// <summary>
  /// The types of notifications that can be requested
  /// </summary>
  [Flags]
  public enum NotificationRequired
  {
    /// <summary>
    /// Provide no notifications
    /// </summary>
    None = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_NONE,
    /// <summary>
    /// Suppress notifications
    /// </summary>
    /// <remarks>This allows previously requested notifications to be removed</remarks>
    Suppress = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_SUPPRESS_NOTIFICATIONS,
    /// <summary>
    /// Get notified when a file is opened
    /// </summary>
    FileOpened = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_OPENED,
    /// <summary>
    /// Get notified when a file is created
    /// </summary>
    FileCreated = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_NEW_FILE_CREATED,
    /// <summary>
    /// Get notified when a file is overwritten
    /// </summary>
    FileOverwritten = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_OVERWRITTEN,
    /// <summary>
    /// Get notified before a file is deleted and be able to prevent the deletion from occurring
    /// </summary>
    PreDelete = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_PRE_DELETE,
    /// <summary>
    /// Get notified before a file is renamed/moved and be able to prevent the rename from occurring
    /// </summary>
    PreRename = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_PRE_RENAME,
    /// <summary>
    /// Get notified before a file is hardlinked and be able to prevent the hardlink from occurring
    /// </summary>
    PreHardlink = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_PRE_SET_HARDLINK,
    /// <summary>
    /// Get notified when a file is renamed
    /// </summary>
    FileRenamed = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_RENAMED,
    /// <summary>
    /// Get notified when a file is hardlinked
    /// </summary>
    FileHardlinked = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_HARDLINK_CREATED,
    /// <summary>
    /// Get notified when file is closed with no modifications applied
    /// </summary>
    FileClosedUnmodified = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_HANDLE_CLOSED_NO_MODIFICATION,
    /// <summary>
    /// Get notified when a file has been modified and closed
    /// </summary>
    FileClosedModified = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_HANDLE_CLOSED_FILE_MODIFIED,
    /// <summary>
    /// Get notified when a file has been deleted
    /// </summary>
    FileClosedDeleted = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_HANDLE_CLOSED_FILE_DELETED,
    /// <summary>
    /// Get notified when a files contents are going to be requested
    /// </summary>
    PreConvertToFull = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_FILE_PRE_CONVERT_TO_FULL,
    /// <summary>
    /// Used to indicate that no changes to existing notifications should occur
    /// </summary>
    UseExisting = PRJ_NOTIFY_TYPES.PRJ_NOTIFY_USE_EXISTING_MASK
  }
}
