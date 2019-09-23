using ProjectedFileSystem.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Interfaces
{
  /// <summary>
  /// A FileSystem is the managed side of an implementation of the concept of a Projected File System
  /// </summary>
  public interface IFileSystem
  {
    /// <summary>
    /// Searches a <paramref name="directoryPath"/> for files and directories contained within
    /// </summary>
    /// <param name="directoryPath">The path to search</param>
    /// <param name="versionInfo">The version of the path that is of interest</param>
    /// <param name="filter">Only return information for paths which pass the filter</param>
    /// <param name="sortOrder">The order in which results should be returned</param>
    /// <param name="cancel">If possible, cancel enumeration without performing any further work</param>
    /// <returns>Provides basic information about each file system object</returns>
    IAsyncEnumerator<BasicInfo> FindContent(
      string directoryPath,
      PlaceholderVersion versionInfo,
      Func<string, bool> filter,
      IComparer<string> sortOrder,
      CancellationToken cancel);

    /// <summary>
    /// Retreives a more complex detail of a file whilst still not having to supply the entire contents
    /// </summary>
    /// <param name="path">The path of interest. The laxest possible matching process should be used and the <paramref name="matcher"/> used to determine actual matches</param>
    /// <param name="matcher">The matcher should be used to determine whether a file name actually matches</param>
    /// <param name="cancelation">Cancel the operation without performing further work</param>
    /// <returns>The more detailed metadata for the file</returns>
    Task<PlaceholderInfo> GetPlaceholder(
      string path,
      Func<string, bool> matcher,
      CancellationToken cancelation);

    /// <summary>
    /// Requests the actual content of a file
    /// </summary>
    /// <param name="path">The path to the file</param>
    /// <param name="version">The specific version of the file to retrieve the content of</param>
    /// <param name="offset">The offset within the file</param>
    /// <param name="length">The length of data within the file</param>
    /// <param name="toWrite">The stream to write data to</param>
    /// <returns>When at least <paramref name="length"/> bytes have been written to <paramref name="toWrite"/> then the task should be completed</returns>
    Task GetFileData(string path, PlaceholderVersion version, long offset, int length, Stream toWrite);
  }
}
