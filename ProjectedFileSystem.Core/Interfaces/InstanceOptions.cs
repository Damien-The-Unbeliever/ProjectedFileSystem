using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Interfaces
{
  /// <summary>
  /// Options that control how an instance of the Projected File System is supported by the System
  /// </summary>
  /// <remarks>https://docs.microsoft.com/en-gb/windows/desktop/projfs/asynchronous-callback-handling describes how these options are used</remarks>
  public struct InstanceOptions
  {
    /// <summary>
    /// The number of pool threads to use
    /// </summary>
    public int PoolThreadCount { get; set; }
    /// <summary>
    /// The number of non-pool threads to use
    /// </summary>
    public int ConcurrentThreadCount { get; set; }
    /// <summary>
    /// Asks the system to provide a negative path cache
    /// </summary>
    public bool NegativePathCache { get; set; }
  }
}
