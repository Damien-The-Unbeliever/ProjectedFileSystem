using ProjectedFileSystem.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectedFileSystem.InMemory
{
  public class SimpleStringFile : SimpleBinaryFile
  {
    public SimpleStringFile(BasicFileInfo basicMetadata, string content, SimpleVersion version) :
      base(basicMetadata,Encoding.UTF8.GetBytes(content),version)
    {
    }
    public SimpleStringFile(BasicFileInfo basicMetadata, string content, int contentId) : this(basicMetadata, content, new SimpleVersion(contentId))
    {

    }
    public SimpleStringFile(BasicFileInfo basicMetadata, string content) : this(basicMetadata, content, 0)
    {

    }
    public SimpleStringFile(string name, string content, SimpleVersion version) : this(new BasicFileInfo(name,content.Length),content,version)
    {
    }
    public SimpleStringFile(string name, string content, int contentId) : this(new BasicFileInfo(name, content.Length), content, contentId)
    {

    }
    public SimpleStringFile(string name, string content) : this(new BasicFileInfo(name, content.Length), content, 0)
    {

    }
  }
}
