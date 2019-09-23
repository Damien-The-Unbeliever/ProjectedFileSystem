using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.FileSystem
{
  /// <summary>
  /// A basic representation of a file system object
  /// </summary>
  public abstract class BasicInfo
  {
    /// <summary>
    /// Initializes the file system information with the minimal information required
    /// </summary>
    /// <param name="name">The name of the file or directory</param>
    protected BasicInfo(string name)
    {
      Name = name;
    }
    /// <summary>
    /// True if this file system object is a directory rather than a file
    /// </summary>
    public abstract bool IsDirectory { get; }
    /// <summary>
    /// The name of the file system object
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// When the file was first created
    /// </summary>
    public virtual DateTime? Created { get; set; }
    /// <summary>
    /// When the file was last accessed
    /// </summary>
    public virtual DateTime? Accessed { get; set; }
    /// <summary>
    /// When the file was last written to
    /// </summary>
    public virtual DateTime? Written { get; set; }
    /// <summary>
    /// When the file changed
    /// </summary>
    public virtual DateTime? Changed { get; set; }
    /// <summary>
    /// The size of the file
    /// </summary>
    public abstract long Size { get; }
    /// <summary>
    /// Basic attributes about the file
    /// </summary>
    public FileAttributes Attributes { get; set; }
  }
}
