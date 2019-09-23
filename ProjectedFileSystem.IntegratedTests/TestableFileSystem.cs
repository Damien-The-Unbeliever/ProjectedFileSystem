using ProjectedFileSystem.Core.Native;
using ProjectedFileSystem.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectedFileSystem.IntegratedTests
{
  internal class TestableFileSystem
  {
    public delegate void StartVirtualizingCallback(String virtualizationRootPath,
      IntPtr callbacks,
      IntPtr instanceContext,
      PRJ_STARTVIRTUALIZING_OPTIONS options,
      out IntPtr namespaceVirtualizationContext);

    public delegate void GetVirtualizationInfoCallback(IntPtr namespaceVirtualizationContext,
      ref PRJ_VIRTUALIZATION_INSTANCE_INFO virtualizationInstanceInfo);

    public delegate void FillDirEntryCallback(
     String fileName,
      PRJ_FILE_BASIC_INFO fileBasicInfo,
      IntPtr dirEntryBufferHandle
    );

    public static SimpleFileSystem SingleFile()
    {
      var borisTxt = new SimpleStringFile("Boris.txt", "Hello world (InMemory)");
      borisTxt.BasicMetadata.Created = DateTime.Today.AddYears(-1);
      borisTxt.BasicMetadata.Written = DateTime.Today;
      var rootDirectory = new SimpleDirectory("",
        borisTxt
      );

      return new SimpleFileSystem(rootDirectory);
    }

    public static SimpleFileSystem MultiFile()
    {
      var borisTxt = new SimpleStringFile("Boris.txt", "Hello world (InMemory)");
      borisTxt.BasicMetadata.Created = DateTime.Today.AddYears(-1);
      borisTxt.BasicMetadata.Written = DateTime.Today;

      var readmeTxt = new SimpleStringFile("Readme.txt", "Nothing important");
      readmeTxt.BasicMetadata.Created = DateTime.Today.AddYears(-2);
      readmeTxt.BasicMetadata.Written = DateTime.Today;

      var rootDirectory = new SimpleDirectory("",
        borisTxt,
        readmeTxt
      );

      return new SimpleFileSystem(rootDirectory);
    }
  }
}
