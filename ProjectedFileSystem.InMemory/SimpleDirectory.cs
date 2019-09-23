using ProjectedFileSystem.Core.FileSystem;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Linq;

namespace ProjectedFileSystem.InMemory
{
  public class SimpleDirectory : SimpleFileSystemObject
  {
    private static readonly DirectorySecurity _defaultSecurity;
    private readonly ConcurrentDictionary<string, SimpleFileSystemObject> _content;
    static SimpleDirectory()
    {
      _defaultSecurity = new DirectorySecurity();
      _defaultSecurity.AddAccessRule(
        new FileSystemAccessRule(
          new SecurityIdentifier(WellKnownSidType.WorldSid, null),
          FileSystemRights.FullControl,
          AccessControlType.Allow));
    }

    public SimpleDirectory(BasicDirectoryInfo basicMetadata, SimpleVersion version, params SimpleFileSystemObject[] content) : base(basicMetadata, version, _defaultSecurity)
    {
      _content = new ConcurrentDictionary<string, SimpleFileSystemObject>(content.ToDictionary(item => item.Name, item=>item));
    }
    public SimpleDirectory(string name, SimpleVersion version, params SimpleFileSystemObject[] content):this(new BasicDirectoryInfo(name), version, content)
    {

    }
    public SimpleDirectory(BasicDirectoryInfo basicMetadata, int contentId, params SimpleFileSystemObject[] content) : this(basicMetadata, new SimpleVersion(contentId), content)
    {
    }
    public SimpleDirectory(string name, int contentId, params SimpleFileSystemObject[] content) : this(new BasicDirectoryInfo(name), new SimpleVersion(contentId), content)
    {

    }
    public SimpleDirectory(BasicDirectoryInfo basicMetadata, params SimpleFileSystemObject[] content) : this(basicMetadata, 0, content)
    {
    }
    public SimpleDirectory(string name, params SimpleFileSystemObject[] content) : this(new BasicDirectoryInfo(name), 0, content)
    {

    }

    public IEnumerable<SimpleFileSystemObject> GetObjects()
    {
      return _content.Values;
    }
    public SimpleFileSystemObject GetObject(string name)
    {
      if (_content.TryGetValue(name, out var item)) return item;
      return null;
    }

    public bool TryRemove(string name)
    {
      return _content.TryRemove(name, out _);
    }

    public bool TryAdd(SimpleFileSystemObject item)
    {
      return _content.TryAdd(item.Name, item);
    }
  }
}
