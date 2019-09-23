using ProjectedFileSystem.Core.Interfaces;
using ProjectedFileSystem.Core.Native;
using System;

namespace ProjectedFileSystem.Core.Impl
{
  internal sealed partial class RunningInstance
  {
    private INotifiableFileSystem Notifiable => FileSystem as INotifiableFileSystem;
    private HRESULT ProcessNotification(
      PRJ_CALLBACK_DATA callbackData,
      bool isDirectory,
      PRJ_NOTIFICATION notification,
      String destinationFileName,
      ref PRJ_NOTIFICATION_PARAMETERS operationParameters
    )
    {
      switch (notification)
      {
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_OPENED:
          return _asyncOperations.ProcessCommandPossibleAsyncWithNotificationMask(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.FileOpened(callbackData.FilePathName, isDirectory,cts.Token), operationParameters);
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_NEW_FILE_CREATED:
          return _asyncOperations.ProcessCommandPossibleAsyncWithNotificationMask(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.FileCreated(callbackData.FilePathName, isDirectory, cts.Token), operationParameters);
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_OVERWRITTEN:
          return _asyncOperations.ProcessCommandPossibleAsyncWithNotificationMask(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.FileOverwritten(callbackData.FilePathName, isDirectory, cts.Token), operationParameters);
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_PRE_DELETE:
          return _asyncOperations.ProcessCommandPossibleAsync(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.PreDelete(callbackData.FilePathName, isDirectory, cts.Token), b => b ? HRESULT.S_OK : HRESULT.E_INVALIDARG);
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_PRE_RENAME:
          return _asyncOperations.ProcessCommandPossibleAsync(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.PreRename(callbackData.FilePathName, destinationFileName, isDirectory, cts.Token), b => b ? HRESULT.S_OK : HRESULT.E_INVALIDARG);
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_PRE_SET_HARDLINK:
          return _asyncOperations.ProcessCommandPossibleAsync(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.PreHardlink(callbackData.FilePathName, destinationFileName, isDirectory, cts.Token), b => b ? HRESULT.S_OK : HRESULT.E_INVALIDARG);
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_RENAMED:
          return _asyncOperations.ProcessCommandPossibleAsyncWithNotificationMask(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.FileRenamed(callbackData.FilePathName, destinationFileName, isDirectory, cts.Token), operationParameters);
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_HARDLINK_CREATED:
          return _asyncOperations.ProcessCommandPossibleAsync(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.FileHardlinked(callbackData.FilePathName, destinationFileName, isDirectory, cts.Token));
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_HANDLE_CLOSED_NO_MODIFICATION:
          return _asyncOperations.ProcessCommandPossibleAsync(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.FileClosed(callbackData.FilePathName, isDirectory, CloseState.Unmodified, cts.Token));
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_HANDLE_CLOSED_FILE_MODIFIED:
          return _asyncOperations.ProcessCommandPossibleAsync(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.FileClosed(callbackData.FilePathName, isDirectory, CloseState.Modified, cts.Token));
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_HANDLE_CLOSED_FILE_DELETED:
          CloseState state = operationParameters.IsFileModified ? CloseState.Modified | CloseState.Deleted : CloseState.Deleted;
          return _asyncOperations.ProcessCommandPossibleAsync(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.FileClosed(callbackData.FilePathName, isDirectory, state, cts.Token));
        case PRJ_NOTIFICATION.PRJ_NOTIFICATION_FILE_PRE_CONVERT_TO_FULL:
          return _asyncOperations.ProcessCommandPossibleAsync(callbackData.NamespaceVirtualizationContext, callbackData.CommandId, cts => Notifiable.PreConvertToFull(callbackData.FilePathName, isDirectory, cts.Token));

      }
      return HRESULT.E_INVALIDARG;
    }
  }
}
