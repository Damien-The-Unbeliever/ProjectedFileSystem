using Moq;
using ProjectedFileSystem.Core.Impl;
using ProjectedFileSystem.Core.Interfaces;
using ProjectedFileSystem.Core.Native;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static ProjectedFileSystem.IntegratedTests.TestableFileSystem;

namespace ProjectedFileSystem.IntegratedTests
{
  public class BasicLifecycle
  {



    [Fact]
    public void CanExecuteStartupAndShutdown()
    {
      //Arrange
      const string RootPath = @"D:\";
      var functions = new Mock<Functions>();
      var nsVirtualizationContext = (IntPtr)4;
      var instanceGuid = Guid.NewGuid();

      functions.Setup(f => f.PrjStartVirtualizing(RootPath, It.IsAny<IntPtr>(), IntPtr.Zero, It.IsAny<PRJ_STARTVIRTUALIZING_OPTIONS>(), out It.Ref<IntPtr>.IsAny))
        .Callback(new StartVirtualizingCallback((String virtualizationRootPath,
          IntPtr callbacks,
          IntPtr instanceContext,
          PRJ_STARTVIRTUALIZING_OPTIONS options,
          out IntPtr namespaceVirtualizationContext) =>
          {
            namespaceVirtualizationContext = nsVirtualizationContext;
          })
        );
      functions.Setup(f => f.PrjGetVirtualizationInstanceInfo(nsVirtualizationContext, ref It.Ref<PRJ_VIRTUALIZATION_INSTANCE_INFO>.IsAny))
        .Callback(new GetVirtualizationInfoCallback((IntPtr namespaceVirtualizationContext,
            ref PRJ_VIRTUALIZATION_INSTANCE_INFO virtualizationInstanceInfo) =>
        {
          virtualizationInstanceInfo.InstanceID = instanceGuid;
          virtualizationInstanceInfo.WriteAlignment = 2048;
        })
      );
      functions.Setup(f => f.PrjStopVirtualizing(nsVirtualizationContext));
      var runnable = new RunnableInstance("Boris", RootPath, instanceGuid, new InstanceOptions(), functions.Object);
      var fs = SingleFile();

      //Act
      var running = runnable.Start(fs);
      running.Shutdown();

      //Assert
      functions.VerifyAll();
      functions.VerifyNoOtherCalls();
      Assert.Same(runnable, running.InstanceOf);
      Assert.Same(fs, running.FileSystem);
    }
  }
}
