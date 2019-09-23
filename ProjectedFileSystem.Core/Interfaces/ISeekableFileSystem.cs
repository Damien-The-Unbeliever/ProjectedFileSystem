using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Interfaces
{
  /// <summary>
  /// A file system that supports seeking for an individual file
  /// </summary>
  public interface ISeekableFileSystem : IFileSystem
  {
    /// <summary>
    /// The file to locate
    /// </summary>
    /// <param name="path">The path of interest. The laxest possible matching process should be used and the <paramref name="matcher"/> used to determine actual matches</param>
    /// <param name="matcher">The matcher should be used to determine whether a file name actually matches</param>
    /// <param name="cancelation">Cancel the operation without performing further work</param>
    /// <returns></returns>
    Task<bool> FileExists(string path, Func<string, bool> matcher, CancellationToken cancelation);
  }
}
