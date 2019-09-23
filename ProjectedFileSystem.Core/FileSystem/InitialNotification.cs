using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.FileSystem
{
  /// <summary>
  /// A notification that it should be possible to receive immediately after an instance starts running
  /// </summary>
  public class InitialNotification
  {
    /// <summary>
    /// Initializes the notification information
    /// </summary>
    /// <param name="path">The path of interest</param>
    /// <param name="notifications">The notifications required</param>
    public InitialNotification(string path, NotificationRequired notifications)
    {
      Path = path;
      Notifications = notifications;
    }
    /// <summary>
    /// The path of interest
    /// </summary>
    public string Path { get; }
    /// <summary>
    /// The notifications required
    /// </summary>
    public NotificationRequired Notifications { get; }
  }
}
