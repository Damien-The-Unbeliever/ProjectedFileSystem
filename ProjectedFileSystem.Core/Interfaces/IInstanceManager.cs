using ProjectedFileSystem.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Interfaces
{
  /// <summary>
  /// Allows instances of virtual file systems to be registered or searched for
  /// </summary>
  public interface IInstanceManager
  {
    /// <summary>
    /// Register a new virtual file system
    /// </summary>
    /// <param name="name">The name for the virtual file system. Any particular <see cref="IInstanceManager"/> should enforce unique naming</param>
    /// <param name="rootPath">The root path in which the virtual file system should appear. Any particular <see cref="IInstanceManager"/> should only allow one instance to be registered per root path</param>
    /// <param name="targetPath">The target path (~~~~I don't get this bit yet)</param>
    /// <param name="defaultOptions">Options to be applied when starting the instance, if no specific overrides are provided</param>
    /// <param name="rootVersionInfo">The version of the root of the file system</param>
    /// <param name="instanceGuid">A unique guid associated with the instance</param>
    /// <returns>An instance that may be started</returns>
    IRunnableInstance Register(string name, string rootPath, string targetPath, InstanceOptions defaultOptions, PlaceholderVersion rootVersionInfo, Guid instanceGuid);
    /// <summary>
    /// Locate a <see cref="IRunnableInstance"/> based on the <paramref name="guid"/> it was registered using
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/> the instance was registered with</param>
    /// <returns>An instance that may be started</returns>
    IRunnableInstance FindByGuid(Guid guid);
    /// <summary>
    /// Locate a <see cref="IRunnableInstance"/> based on the <paramref name="rootPath"/> it was registered using
    /// </summary>
    /// <param name="rootPath">The path the instance was registered with</param>
    /// <returns>An instance that may be started</returns>
    IRunnableInstance FindByRootPath(string rootPath);
    /// <summary>
    /// Locate a <see cref="IRunnableInstance"/> based on the <paramref name="name"/> it was registered using
    /// </summary>
    /// <param name="name">The name the instance was registered with</param>
    /// <returns>An instance that may be started</returns>
    IRunnableInstance FindByName(string name);

    /// <summary>
    /// Remove registration for an instance
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/> the instance was registered with</param>
    /// <remarks>In practice, the rootPath of the instance will be removed</remarks>
    void Deregister(Guid guid);
  }
}
