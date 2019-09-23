using ProjectedFileSystem.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace ProjectedFileSystem.InMemory
{
  public abstract class SimpleFile : SimpleFileSystemObject
  {
    private readonly Func<long, int, ReadOnlyMemory<byte>> _contentFetcher;
    private static readonly FileSecurity _defaultSecurity;
    static SimpleFile()
    {
      _defaultSecurity = new FileSecurity();
      _defaultSecurity.AddAccessRule(
        new FileSystemAccessRule(
          new SecurityIdentifier(WellKnownSidType.WorldSid, null),
          FileSystemRights.FullControl,
          AccessControlType.Allow));
    }

    protected SimpleFile(BasicFileInfo basicMetadata, Func<long,int, ReadOnlyMemory<byte>> contentFetcher, SimpleVersion version) : base(basicMetadata,version,_defaultSecurity)
    {
      _contentFetcher = contentFetcher;
    }




    public ReadOnlyMemory<byte> GetContent(long offset, int length) => _contentFetcher(offset, length);


  }
}
