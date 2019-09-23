using ProjectedFileSystem.Core;
using ProjectedFileSystem.Core.FileSystem;
using ProjectedFileSystem.Core.Interfaces;
using ProjectedFileSystem.InMemory;
using System;
using System.IO;
using System.Security.AccessControl;

namespace ProjectedFileSystem.Scratch
{
  class Program
  {
    static void Main()
    {
      var noddyDir = Path.GetFullPath("Noddy");
      if (!Directory.Exists(noddyDir))
      {
        Directory.CreateDirectory(noddyDir);
      }
      IInstanceManager fileManager = new FileBasedInstanceManager("Noddy.reg");
      var instance = fileManager.FindByName("Noddy");
      var version = new PlaceholderVersion(new byte[0], new byte[0]);
      if (instance == null)
      {
        var instanceGuid = Guid.NewGuid();
        instance = fileManager.Register("Noddy", noddyDir, null, default, version, instanceGuid);
      }

      var borisTxt = new SimpleStringFile("Boris.txt", "Hello world (InMemory)");
      borisTxt.BasicMetadata.Created = DateTime.Today.AddYears(-1);
      borisTxt.BasicMetadata.Written = DateTime.Today;
      var rootDirectory = new SimpleDirectory("",
        borisTxt,
        new SimpleDirectory("Frubert")
      );

      using (var running = instance.Start(new SimpleFileSystem(rootDirectory)))
      {
        Console.WriteLine("File system is running");
        Console.WriteLine("Press return to shut down");
        Console.ReadLine();
      }
    }
  }
}
