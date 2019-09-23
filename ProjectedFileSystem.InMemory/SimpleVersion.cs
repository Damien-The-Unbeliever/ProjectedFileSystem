using ProjectedFileSystem.Core.FileSystem;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace ProjectedFileSystem.InMemory
{
  public sealed class SimpleVersion : PlaceholderVersion
  {
    public SimpleVersion(int contentId) : this(0, contentId)
    {

    }
    public SimpleVersion(int providerId, int contentId) : base(BitConverter.GetBytes(providerId),BitConverter.GetBytes(contentId))
    {

    }
  }
}
