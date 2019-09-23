using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Interfaces
{
  /// <summary>
  /// A running instance of the Projected File System
  /// </summary>
  public interface IRunningInstance : IDisposable
  {
    /// <summary>
    /// The managed side of the implementation
    /// </summary>
    IFileSystem FileSystem { get; }
    /// <summary>
    /// Shuts down the running instance
    /// </summary>
    void Shutdown();
    /// <summary>
    /// Indicates which registered instance this is an instance of
    /// </summary>
    IRunnableInstance InstanceOf { get; }
  }
}
