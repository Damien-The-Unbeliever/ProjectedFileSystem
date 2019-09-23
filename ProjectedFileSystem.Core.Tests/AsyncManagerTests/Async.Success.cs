using Moq;
using ProjectedFileSystem.Core.FileSystem;
using ProjectedFileSystem.Core.Impl;
using ProjectedFileSystem.Core.Native;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectedFileSystem.Core.Tests.AsyncManagerTests
{
  public partial class Async
  {
    public class Success
    {
      [Fact]
      public async Task Task_No_Transform_Is_S_OK()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.S_OK, IntPtr.Zero));
        var tcs = new TaskCompletionSource<int>();
        var target = new AsyncManager(functions.Object);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (cts) => tcs.Task);
        tcs.SetResult(1);
        await Task.Delay(50); 

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }
      [Fact]
      public async Task ValueTask_No_Transform_Is_S_OK()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.S_OK, IntPtr.Zero));
        var tcs = new TaskCompletionSource<int>();
        var target = new AsyncManager(functions.Object);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (cts) => new ValueTask(tcs.Task));
        tcs.SetResult(1);
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }
      [Fact]
      public async Task Task_With_Transform_Is_S_OK()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.S_OK, IntPtr.Zero));
        var tcs = new TaskCompletionSource<int>();
        var target = new AsyncManager(functions.Object);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (cts) => tcs.Task, (value) => HRESULT.S_OK);
        tcs.SetResult(4);
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }
      [Fact]
      public async Task Task_With_Transform_Is_Other_Result()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.S_FALSE, IntPtr.Zero));
        var tcs = new TaskCompletionSource<int>();
        var target = new AsyncManager(functions.Object);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (cts) => tcs.Task, (value) => HRESULT.S_FALSE);
        tcs.SetResult(4);
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }

      [Fact]
      public async Task Task_With_NotificationMask_Is_S_OK()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.S_OK, It.IsAny<IntPtr>()));
        var tcs = new TaskCompletionSource<NotificationRequired>();
        var target = new AsyncManager(functions.Object);

        var parms = new PRJ_NOTIFICATION_PARAMETERS();

        //Act
        var hr = target.ProcessCommandPossibleAsyncWithNotificationMask(namespaceCtx, 1, (cts) => tcs.Task, parms);
        tcs.SetResult(NotificationRequired.FileCreated);
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        Assert.Equal(PRJ_NOTIFY_TYPES.PRJ_NOTIFY_NONE, parms.NotificationMask);
        functions.VerifyAll();
      }
    }
  }
}
