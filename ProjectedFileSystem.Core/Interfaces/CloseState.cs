using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Interfaces
{
  /// <summary>
  /// When notified that a file is closed, it may have had a number of actions performed against it before closure
  /// </summary>
  [Flags]
  public enum CloseState
  {
    /// <summary>
    /// The file retains the same state it had previously
    /// </summary>
    Unmodified = 0,
    /// <summary>
    /// The file's contents have been updated
    /// </summary>
    Modified = 1,
    /// <summary>
    /// The file has been removed
    /// </summary>
    Deleted = 2
  }
}
