using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.FileSystem
{
  /// <summary>
  /// Information about an alternative data stream to be contained within a file
  /// </summary>
  public class AlternativeDataStreamInfo
  {
    /// <summary>
    /// Initializes the stream information
    /// </summary>
    /// <param name="name">The name of the alternative data stream</param>
    /// <param name="size">The size of the alternative data stream</param>
    /// <param name="allocationSize">The allocation size for the data stream</param>
    public AlternativeDataStreamInfo(string name, long size, long allocationSize)
    {
      Name = name;
      Size = size;
      AllocationSize = allocationSize;
    }
    /// <summary>
    /// The name of the alternative data stream
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// The size of the alternative data stream
    /// </summary>
    public long Size { get; }
    /// <summary>
    /// The allocation size for the data stream
    /// </summary>
    public long AllocationSize { get; }
  }
}
