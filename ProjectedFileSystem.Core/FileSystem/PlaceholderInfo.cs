using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace ProjectedFileSystem.Core.FileSystem
{
  /// <summary>
  /// A placeholder is a more fleshed out set of metadata for a file
  /// </summary>
  public class PlaceholderInfo
  {
    private readonly AlternativeDataStreamInfo[] _alternativeStreams;
    /// <summary>
    /// Initializes the placeholder information
    /// </summary>
    /// <param name="basicInfo">The basic information for the file</param>
    /// <param name="destinationFileName">The specific name of the file defined by the provider</param>
    /// <param name="version">The specific version of the file</param>
    /// <param name="security">The security descriptor that should apply to the file</param>
    /// <param name="alternativeStreams">Alternative data streams that are part of the file</param>
    public PlaceholderInfo(BasicInfo basicInfo, string destinationFileName, PlaceholderVersion version, FileSystemSecurity security, params AlternativeDataStreamInfo[] alternativeStreams)
    {
      BasicInfo = basicInfo;
      DestinationFileName = destinationFileName;
      Security = security;
      Version = version;
      _alternativeStreams = alternativeStreams;
    }
    /// <summary>
    /// The basic information for the file
    /// </summary>
    public BasicInfo BasicInfo { get; }
    /// <summary>
    /// The specific name of the file defined by the provider
    /// </summary>
    public string DestinationFileName { get; }
    /// <summary>
    /// The security descriptor that should apply to the file
    /// </summary>
    public FileSystemSecurity Security { get; }
    /// <summary>
    /// The specific version of the file
    /// </summary>
    public PlaceholderVersion Version { get; }
    /// <summary>
    /// Alternative data streams for the file
    /// </summary>
    public IEnumerable<AlternativeDataStreamInfo> AlternateStreams { get { return _alternativeStreams.AsEnumerable(); } }
  }
}
