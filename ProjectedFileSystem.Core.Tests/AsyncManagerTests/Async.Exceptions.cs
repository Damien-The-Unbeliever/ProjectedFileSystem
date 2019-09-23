using Moq;
using ProjectedFileSystem.Core.FileSystem;
using ProjectedFileSystem.Core.Impl;
using ProjectedFileSystem.Core.Native;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectedFileSystem.Core.Tests.AsyncManagerTests
{
  public partial class Async
  {
    public class Exceptions
    {
      [Fact]
      public async Task Direct_Task_No_Transform_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.E_INVALIDARG, IntPtr.Zero));
        var tcs = new TaskCompletionSource<NotificationRequired>();
        var target = new AsyncManager(functions.Object);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (Func<CancellationTokenSource, Task>)((cts) => tcs.Task));
        tcs.SetException(new Exception());
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }

      [Fact]
      public async Task Direct_ValueTask_No_Transform_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.E_INVALIDARG, IntPtr.Zero));
        var tcs = new TaskCompletionSource<NotificationRequired>();
        var target = new AsyncManager(functions.Object);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (Func<CancellationTokenSource, ValueTask>)((cts) => new ValueTask(tcs.Task)));
        tcs.SetException(new Exception());
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }

      [Fact]
      public async Task Direct_Task_Transform_With_Error_In_Kickoff_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.E_INVALIDARG, IntPtr.Zero));
        var tcs = new TaskCompletionSource<int>();
        var target = new AsyncManager(functions.Object);

        //Act
        var hr = target.ProcessCommandPossibleAsync<int>(namespaceCtx, 1, (cts) => tcs.Task, UncalledCallback<int>);
        tcs.SetException(new Exception());
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }

      [ExcludeFromCodeCoverage]
      private static HRESULT UncalledCallback<TInput>(TInput value)
      {
        throw new Exception("Test failed if callback invoked");
      }

      [Fact]
      public async Task Direct_Task_Transform_With_Error_In_Transform_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.E_INVALIDARG, IntPtr.Zero));
        var tcs = new TaskCompletionSource<int>();
        var target = new AsyncManager(functions.Object);

        //Act
        var hr = target.ProcessCommandPossibleAsync<int>(namespaceCtx, 1, (cts) => tcs.Task, (value) => throw new Exception());
        tcs.SetResult(4);
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }

      [Fact]
      public async Task Direct_Task_With_NotificationMask_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.E_INVALIDARG, IntPtr.Zero));
        var tcs = new TaskCompletionSource<NotificationRequired>();
        var target = new AsyncManager(functions.Object);
        var parms = new PRJ_NOTIFICATION_PARAMETERS();

        //Act
        var hr = target.ProcessCommandPossibleAsyncWithNotificationMask(namespaceCtx, 1, (cts) => tcs.Task, parms);
        tcs.SetException(new Exception());
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }

      [Fact]
      public async Task Indirect_Task_No_Transform_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.E_INVALIDARG, IntPtr.Zero));
        var tcs = new TaskCompletionSource<NotificationRequired>();
        var target = new AsyncManager(functions.Object);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (Func<CancellationTokenSource, Task>)((cts) => tcs.Task));
        tcs.SetException(new Exception());
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }

      [Fact]
      public async Task Indirect_Task_Transform_With_Error_In_Kickoff_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.E_INVALIDARG, IntPtr.Zero));
        var tcs = new TaskCompletionSource<int>();
        var target = new AsyncManager(functions.Object);

        //Act
        var hr = target.ProcessCommandPossibleAsync<int>(namespaceCtx, 1, (cts) => tcs.Task, UncalledCallback<int>);
        tcs.SetException(new Exception());
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }

      [Fact]
      public async Task Indirect_Task_With_NotificationMask_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        functions.Setup(f => f.PrjCompleteCommand(namespaceCtx, 1, HRESULT.E_INVALIDARG, IntPtr.Zero));
        var tcs = new TaskCompletionSource<NotificationRequired>();
        var target = new AsyncManager(functions.Object);
        var parms = new PRJ_NOTIFICATION_PARAMETERS();

        //Act
        var hr = target.ProcessCommandPossibleAsyncWithNotificationMask(namespaceCtx, 1, (cts) => tcs.Task, parms);
        tcs.SetException(new Exception());
        await Task.Delay(50);

        //Assert
        Assert.Equal(HRESULT.ERROR_IO_PENDING, hr);
        functions.VerifyAll();
      }
    }
  }
}
