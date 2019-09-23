using ProjectedFileSystem.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Interfaces
{
  /// <summary>
  /// A file system that wants to be notified when file system actions are performed by the system
  /// </summary>
  public interface INotifiableFileSystem : IFileSystem
  {
    /// <summary>
    /// Indicates the set of paths and notifications required when the file system is started
    /// </summary>
    /// <returns></returns>
    IEnumerable<InitialNotification> StartingNotifications();
    /// <summary>
    /// A file has been opened
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <param name="isDirectory">If the path is for a directory rather than a file</param>
    /// <returns>The notifications that are now required for this file</returns>
    Task<NotificationRequired> FileOpened(string path, bool isDirectory, CancellationToken cancellation);
    /// <summary>
    /// A file has been created
    /// </summary>
    /// <param name="path">The path to the new file</param>
    /// <param name="isDirectory">If the path is for a directory rather than a file</param>
    /// <returns>The notifications that are now required for this file</returns>
    Task<NotificationRequired> FileCreated(string path, bool isDirectory, CancellationToken cancellation);
    /// <summary>
    /// A file has been overwritten
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <param name="isDirectory">If the path is for a directory rather than a file</param>
    /// <returns>The notifications that are now required for this file</returns>
    Task<NotificationRequired> FileOverwritten(string path, bool isDirectory, CancellationToken cancellation);
    /// <summary>
    /// A file is going to be deleted
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <param name="isDirectory">If the path is for a directory rather than a file</param>
    /// <returns>Return false to prevent the deletion from occurring</returns>
    Task<bool> PreDelete(string path, bool isDirectory, CancellationToken cancellation);
    /// <summary>
    /// A file is going to be moved/renamed
    /// </summary>
    /// <param name="path">The existing path to the file</param>
    /// <param name="destinationPath">The path where the file will appear if the rename occurs</param>
    /// <param name="isDirectory">If the path is for a directory rather than a file</param>
    /// <returns>Return false to prevent the rename from occurring</returns>
    /// <remarks>
    /// <para>If <paramref name="path"/> is null or empty, the file is being moved into the file system from outside</para>
    /// <para>If <paramref name="destinationPath"/> is null or empty, the file is being moved out of the file system to somewhere else</para>
    /// </remarks>
    Task<bool> PreRename(string path, string destinationPath, bool isDirectory, CancellationToken cancellation);
    /// <summary>
    /// A file is going to be hard linked
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <param name="destinationPath">The path where the link will be created</param>
    /// <param name="isDirectory">If the path is for a directory rather than a file</param>
    /// <returns>Return false to prevent the hardlink from occurring</returns>
    /// <remarks>
    /// <para>If <paramref name="path"/> is null or empty, the file is being linked to is outside the file system</para>
    /// <para>If <paramref name="destinationPath"/> is null or empty, the link is being created outside the file system</para>
    /// </remarks>
    Task<bool> PreHardlink(string path, string destinationPath, bool isDirectory, CancellationToken cancellation);
    /// <summary>
    /// A file has been moved/renamed
    /// </summary>
    /// <param name="path">The old path to the file</param>
    /// <param name="destinationPath">The new path to the file</param>
    /// <param name="isDirectory">If the path is for a directory rather than a file</param>
    /// <returns>The notifications that are now required for this file</returns>
    /// <remarks>
    /// <para>If <paramref name="path"/> is null or empty, the file has been moved into the file system from outside</para>
    /// <para>If <paramref name="destinationPath"/> is null or empty, the file has been moved out of the file system to somewhere else</para>
    /// </remarks>
    Task<NotificationRequired> FileRenamed(string path, string destinationPath, bool isDirectory, CancellationToken cancellation);
    /// <summary>
    /// A file has been hard linked
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <param name="destinationPath">The path where the link has been created</param>
    /// <param name="isDirectory">If the path is for a directory rather than a file</param>
    /// <returns></returns>
    /// <remarks>
    /// <para>If <paramref name="path"/> is null or empty, the linked file is outside the file system</para>
    /// <para>If <paramref name="destinationPath"/> is null or empty, the link is outside the file system</para>
    /// </remarks>
    Task FileHardlinked(string path, string destinationPath, bool isDirectory, CancellationToken cancellation);
    /// <summary>
    /// A file has been closed
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <param name="isDirectory">If the path is for a directory rather than a file</param>
    /// <param name="state">What was done to the file before closure</param>
    /// <returns></returns>
    Task FileClosed(string path, bool isDirectory, CloseState state, CancellationToken cancellation);
    /// <summary>
    /// The contents of a file will be requested shortly
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <param name="isDirectory">If the path is for a directory rather than a file</param>
    /// <returns></returns>
    Task PreConvertToFull(string path, bool isDirectory, CancellationToken cancellation);
  }
}
;