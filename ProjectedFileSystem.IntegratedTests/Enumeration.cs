using Moq;
using ProjectedFileSystem.Core.Impl;
using ProjectedFileSystem.Core.Interfaces;
using ProjectedFileSystem.Core.Native;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static ProjectedFileSystem.IntegratedTests.TestableFileSystem;

namespace ProjectedFileSystem.IntegratedTests
{
  public class Enumeration
  {
    [Fact]
    public async Task QuickEnumeration_PlentifulBuffer_Successful()
    {
      //Arrange
      const string RootPath = @"D:\";
      var functions = new Mock<Functions>();
      var nsVirtualizationContext = (IntPtr)4;
      var instanceGuid = Guid.NewGuid();
      var tcsStarted = new TaskCompletionSource<IntPtr>();
      var enumerationId = Guid.NewGuid();
      var callbackData = new PRJ_CALLBACK_DATA
      {
        VersionInfo = new PRJ_PLACEHOLDER_VERSION_INFO()
      };
      var dirBuffer = (IntPtr)99;

      functions.Setup(f => f.PrjStartVirtualizing(RootPath, It.IsAny<IntPtr>(), IntPtr.Zero, It.IsAny<PRJ_STARTVIRTUALIZING_OPTIONS>(), out It.Ref<IntPtr>.IsAny))
        .Callback(new StartVirtualizingCallback((String virtualizationRootPath,
          IntPtr callbacks,
          IntPtr instanceContext,
          PRJ_STARTVIRTUALIZING_OPTIONS options,
          out IntPtr namespaceVirtualizationContext) =>
        {
          tcsStarted.SetResult(callbacks);
          namespaceVirtualizationContext = nsVirtualizationContext;
        })
        ).Returns(() => HRESULT.S_OK);
      functions.Setup(f => f.PrjFileNameMatch("Boris.txt", null)).Returns(true);
      functions.Setup(f => f.PrjFillDirEntryBuffer("Boris.txt", It.IsAny<PRJ_FILE_BASIC_INFO>(), dirBuffer)).Returns(HRESULT.S_OK);
      ConfigureVirtualizationInfo(functions, nsVirtualizationContext, instanceGuid);
      var runnable = new RunnableInstance("Boris", RootPath, instanceGuid, new InstanceOptions(), functions.Object);
      var fs = TestableFileSystem.SingleFile();

      //Act
      using (var running = runnable.Start(fs))
      {
        var callbacks = NativeBuffer<PRJ_CALLBACKS>.Recover(await tcsStarted.Task);
        var hr = callbacks.StartDirectoryEnumerationCallback(callbackData, enumerationId);
        Assert.Equal(HRESULT.S_OK, hr);
        hr = callbacks.GetDirectoryEnumerationCallback(callbackData, enumerationId, null, dirBuffer);
        Assert.Equal(HRESULT.S_OK, hr);
        hr = callbacks.EndDirectoryEnumerationCallback(callbackData, enumerationId);
        Assert.Equal(HRESULT.S_OK, hr);

        //Assert
        functions.VerifyAll();
      }
    }

    [Fact]
    public async Task QuickEnumeration_SmallBuffer_MultiFile_Successful()
    {
      //Arrange
      const string RootPath = @"D:\";
      var functions = new Mock<Functions>();
      var nsVirtualizationContext = (IntPtr)4;
      var instanceGuid = Guid.NewGuid();
      var tcsStarted = new TaskCompletionSource<IntPtr>();
      var enumerationId = Guid.NewGuid();
      var callbackData = new PRJ_CALLBACK_DATA
      {
        VersionInfo = new PRJ_PLACEHOLDER_VERSION_INFO()
      };
      var dirBuffer1 = (IntPtr)99;
      var dirBuffer2 = (IntPtr)713;

      functions.Setup(f => f.PrjStartVirtualizing(RootPath, It.IsAny<IntPtr>(), IntPtr.Zero, It.IsAny<PRJ_STARTVIRTUALIZING_OPTIONS>(), out It.Ref<IntPtr>.IsAny))
        .Callback(new StartVirtualizingCallback((String virtualizationRootPath,
          IntPtr callbacks,
          IntPtr instanceContext,
          PRJ_STARTVIRTUALIZING_OPTIONS options,
          out IntPtr namespaceVirtualizationContext) =>
        {
          tcsStarted.SetResult(callbacks);
          namespaceVirtualizationContext = nsVirtualizationContext;
        })
        ).Returns(() => HRESULT.S_OK);
      functions.Setup(f => f.PrjFileNameMatch(It.IsAny<string>(), null)).Returns(true);
      functions.SetupSequence(f => f.PrjFillDirEntryBuffer("Boris.txt", It.IsAny<PRJ_FILE_BASIC_INFO>(), dirBuffer1))
        .Returns(HRESULT.S_OK)
        .Returns(HRESULT.ERROR_INSUFFICIENT_BUFFER)
        .Returns(HRESULT.S_OK);
      ConfigureVirtualizationInfo(functions, nsVirtualizationContext, instanceGuid);
      var runnable = new RunnableInstance("Boris", RootPath, instanceGuid, new InstanceOptions(), functions.Object);
      var fs = TestableFileSystem.MultiFile();

      //Act
      using (var running = runnable.Start(fs))
      {
        var callbacks = NativeBuffer<PRJ_CALLBACKS>.Recover(await tcsStarted.Task);
        var hr = callbacks.StartDirectoryEnumerationCallback(callbackData, enumerationId);
        Assert.Equal(HRESULT.S_OK, hr);
        hr = callbacks.GetDirectoryEnumerationCallback(callbackData, enumerationId, null, dirBuffer1);
        Assert.Equal(HRESULT.S_OK, hr);
        hr = callbacks.GetDirectoryEnumerationCallback(callbackData, enumerationId, null, dirBuffer2);
        Assert.Equal(HRESULT.S_OK, hr);
        hr = callbacks.EndDirectoryEnumerationCallback(callbackData, enumerationId);
        Assert.Equal(HRESULT.S_OK, hr);

        //Assert
        functions.VerifyAll();
      }
    }

    [Fact]
    public async Task QuickEnumeration_NoBuffer_Fails()
    {
      //Arrange
      const string RootPath = @"D:\";
      var functions = new Mock<Functions>();
      var nsVirtualizationContext = (IntPtr)4;
      var instanceGuid = Guid.NewGuid();
      var tcsStarted = new TaskCompletionSource<IntPtr>();
      var enumerationId = Guid.NewGuid();
      var callbackData = new PRJ_CALLBACK_DATA
      {
        VersionInfo = new PRJ_PLACEHOLDER_VERSION_INFO()
      };
      var dirBuffer = (IntPtr)99;

      functions.Setup(f => f.PrjStartVirtualizing(RootPath, It.IsAny<IntPtr>(), IntPtr.Zero, It.IsAny<PRJ_STARTVIRTUALIZING_OPTIONS>(), out It.Ref<IntPtr>.IsAny))
        .Callback(new StartVirtualizingCallback((String virtualizationRootPath,
          IntPtr callbacks,
          IntPtr instanceContext,
          PRJ_STARTVIRTUALIZING_OPTIONS options,
          out IntPtr namespaceVirtualizationContext) =>
        {
          tcsStarted.SetResult(callbacks);
          namespaceVirtualizationContext = nsVirtualizationContext;
        })
        ).Returns(() => HRESULT.S_OK);
      functions.Setup(f => f.PrjFileNameMatch("Boris.txt", null)).Returns(true);
      functions.Setup(f => f.PrjFillDirEntryBuffer("Boris.txt", It.IsAny<PRJ_FILE_BASIC_INFO>(), dirBuffer)).Returns(HRESULT.ERROR_INSUFFICIENT_BUFFER);
      ConfigureVirtualizationInfo(functions, nsVirtualizationContext, instanceGuid);
      var runnable = new RunnableInstance("Boris", RootPath, instanceGuid, new InstanceOptions(), functions.Object);
      var fs = TestableFileSystem.SingleFile();

      //Act
      using (var running = runnable.Start(fs))
      {
        var callbacks = NativeBuffer<PRJ_CALLBACKS>.Recover(await tcsStarted.Task);
        var hr = callbacks.StartDirectoryEnumerationCallback(callbackData, enumerationId);
        Assert.Equal(HRESULT.S_OK, hr);
        hr = callbacks.GetDirectoryEnumerationCallback(callbackData, enumerationId, null, dirBuffer);
        Assert.NotEqual(HRESULT.S_OK, hr);
        hr = callbacks.EndDirectoryEnumerationCallback(callbackData, enumerationId);
        Assert.Equal(HRESULT.S_OK, hr);

        //Assert
        functions.VerifyAll();
      }
    }

    private static void ConfigureVirtualizationInfo(Mock<Functions> functions, IntPtr nsVirtualizationContext, Guid instanceGuid)
    {
      functions.Setup(f => f.PrjGetVirtualizationInstanceInfo(nsVirtualizationContext, ref It.Ref<PRJ_VIRTUALIZATION_INSTANCE_INFO>.IsAny))
        .Callback(new GetVirtualizationInfoCallback((IntPtr namespaceVirtualizationContext,
            ref PRJ_VIRTUALIZATION_INSTANCE_INFO virtualizationInstanceInfo) =>
        {
          virtualizationInstanceInfo.InstanceID = instanceGuid;
          virtualizationInstanceInfo.WriteAlignment = 2048;
        })
      );
    }
  }
}
