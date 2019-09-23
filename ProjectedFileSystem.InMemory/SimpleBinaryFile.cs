using ProjectedFileSystem.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectedFileSystem.InMemory
{
  public class SimpleBinaryFile : SimpleFile
  {
    public SimpleBinaryFile(BasicFileInfo basicMetadata, byte[] content, SimpleVersion version) :
      base(basicMetadata,(offset,length)=> new ReadOnlyMemory<byte>(content, (int)offset, length),version)
    {
    }
    public SimpleBinaryFile(BasicFileInfo basicMetadata, byte[] content, int contentId) : this(basicMetadata, content, new SimpleVersion(contentId))
    {

    }
    public SimpleBinaryFile(BasicFileInfo basicMetadata, byte[] content) : this(basicMetadata, content, 0)
    {

    }
    public SimpleBinaryFile(string name, byte[] content, SimpleVersion version) : this(new BasicFileInfo(name,content.Length),content,version)
    {
    }
    public SimpleBinaryFile(string name, byte[] content, int contentId) : this(new BasicFileInfo(name, content.Length), content, contentId)
    {

    }
    public SimpleBinaryFile(string name, byte[] content) : this(new BasicFileInfo(name, content.Length), content, 0)
    {

    }
  }
}
