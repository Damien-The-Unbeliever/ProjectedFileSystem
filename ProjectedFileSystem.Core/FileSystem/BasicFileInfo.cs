using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.FileSystem
{
  /// <summary>
  /// A file system object that is specifically a file
  /// </summary>
  public class BasicFileInfo : BasicInfo
  {
    /// <summary>
    /// Initializes basic information about the file
    /// </summary>
    /// <param name="name">The name of the file</param>
    /// <param name="size">The size of the file</param>
    public BasicFileInfo(string name, long size) : base(name)
    {
      Size = size;
    }
    /// <summary>
    /// No file is a directory, so this is overridden to always return false
    /// </summary>
    public override bool IsDirectory => false;
    /// <summary>
    /// The size of the file
    /// </summary>
    public override long Size { get; }
  }
}
