using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.FileSystem
{
  /// <summary>
  /// A file system object that is specifically a directory
  /// </summary>
  public class BasicDirectoryInfo : BasicInfo
  {
    /// <summary>
    /// Initializes basic information about the directory
    /// </summary>
    /// <param name="name">The name of the directory</param>
    public BasicDirectoryInfo(string name) : base(name)
    {
    }

    /// <summary>
    /// Every directory is a directory so this is overridden to return true
    /// </summary>
    public override bool IsDirectory => true;
    /// <summary>
    /// The "size" of all directories is 0
    /// </summary>
    public override long Size => 0;
  }
}
