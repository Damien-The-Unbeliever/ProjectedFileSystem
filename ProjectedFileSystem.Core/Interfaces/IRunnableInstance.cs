using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Interfaces
{
  /// <summary>
  /// An instance of a Projected File System that is registered with the system and ready to run
  /// </summary>
  public interface IRunnableInstance
  {
    /// <summary>
    /// The name under which the instance was registered
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Start the file system running whilst specifying new options for running the instance
    /// </summary>
    /// <param name="fileSystem">An implementation of the managed side of the file system</param>
    /// <param name="overrideOptions">The options to apply when starting the instance</param>
    /// <returns>The running instance</returns>
    IRunningInstance Start(IFileSystem fileSystem, InstanceOptions overrideOptions);
    /// <summary>
    /// Start the file system running using the <see cref="DefaultOptions"/>
    /// </summary>
    /// <param name="fileSystem">An implementation of the managed side of the file system</param>
    /// <returns>The running instance</returns>
    IRunningInstance Start(IFileSystem fileSystem);
    /// <summary>
    /// The <see cref="Guid"/> that uniquely identifies the instance
    /// </summary>
    Guid InstanceGuid { get; }
    /// <summary>
    /// The root path on the real file system under which this instance operates
    /// </summary>
    string RootPath { get; }
    /// <summary>
    /// The options that will be used if unspecified during a call to <see cref="Start(IFileSystem)"/>
    /// </summary>
    /// <remarks>These options were specified during registration</remarks>
    InstanceOptions DefaultOptions { get; }
  }
}
