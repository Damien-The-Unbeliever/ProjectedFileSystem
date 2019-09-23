using ProjectedFileSystem.Core.FileSystem;
using ProjectedFileSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectedFileSystem.InMemory
{
  public class SimpleFileSystem : IFileSystem, ISeekableFileSystem
  {
    private readonly SimpleDirectory _root;
    public SimpleFileSystem(SimpleDirectory root)
    {
      _root = root;
    }

    async IAsyncEnumerator<BasicInfo> IFileSystem.FindContent(string directoryPath, PlaceholderVersion versionInfo, Func<string, bool> filter, IComparer<string> sortOrder, CancellationToken cancel)
    {
      if (!(ChasePath(directoryPath) is SimpleDirectory directory)) yield break;
      if (directory.Version != versionInfo) yield break;
      foreach (var item in directory.GetObjects().OrderBy(no => no.Name, sortOrder).Where(no => filter(no.Name)))
      {
        yield return item.BasicMetadata;
      }
    }

    async Task IFileSystem.GetFileData(string path, PlaceholderVersion version, long offset, int length, Stream toWrite)
    {
      await Task.Yield();
      if (!(ChasePath(path) is SimpleFile file)) return;
      if (file.Version != version) return;
      toWrite.Write(file.GetContent(offset, length).Span);
    }

    async Task<PlaceholderInfo> IFileSystem.GetPlaceholder(string path, Func<string, bool> matcher, CancellationToken cancelation)
    {
      await Task.Yield();
      var item = ChasePath(path);
      if (item == null) return null;
      return item.DetailedMetadata;
    }
    async Task<bool> ISeekableFileSystem.FileExists(string path, Func<string, bool> matcher, CancellationToken cancelation)
    {
      await Task.Yield();
      var fso = ChasePath(path);
      if (fso == null) return false;
      return matcher(fso.Name);
    }
    private SimpleFileSystemObject ChasePath(string path)
    {
      if (string.IsNullOrEmpty(path)) return _root;
      var visited = new List<SimpleFileSystemObject>();
      var splitPath = path.Split(Path.DirectorySeparatorChar);
      visited.Add(_root);
      foreach (var pathElement in splitPath)
      {
        if (pathElement == ".") { }
        else if (pathElement == "..")
        {
          visited.RemoveAt(visited.Count);
          if (visited.Count == 0) return null;
        }
        else
        {
          if (!(visited[visited.Count - 1] is SimpleDirectory currentDir)) return null;
          var newItem = currentDir.GetObject(pathElement);
          if (newItem == null) return null;
          visited.Add(newItem);
        }
      }
      return visited[visited.Count - 1];
    }


  }
}
