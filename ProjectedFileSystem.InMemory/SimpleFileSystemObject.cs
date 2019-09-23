using ProjectedFileSystem.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace ProjectedFileSystem.InMemory
{
  public abstract class SimpleFileSystemObject
  {
    public BasicInfo BasicMetadata { get; }
    public PlaceholderInfo DetailedMetadata => _detailedMetadata.Value;
    public string Name => BasicMetadata.Name;
    public PlaceholderVersion Version => _version;

    private readonly Lazy<PlaceholderInfo> _detailedMetadata;
    private readonly SimpleVersion _version;

    protected SimpleFileSystemObject(BasicInfo basicMetadata, SimpleVersion version, FileSystemSecurity security)
    {
      BasicMetadata = basicMetadata;
      _version = version;
      _detailedMetadata = new Lazy<PlaceholderInfo>(() => new PlaceholderInfo(BasicMetadata, BasicMetadata.Name, _version, security), true);

    }
  }
}
