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
  public partial class Sync
  {
    public class Exceptions
    {
      [Fact]
      public void Direct_Task_No_Transform_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (Func<CancellationTokenSource, Task>)((cts) => throw new Exception()));

        //Assert
        Assert.Equal(HRESULT.E_INVALIDARG, hr);
      }

      [Fact]
      public void Direct_ValueTask_No_Transform_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (Func<CancellationTokenSource, ValueTask>)((cts) => throw new Exception()));

        //Assert
        Assert.Equal(HRESULT.E_INVALIDARG, hr);
      }

      [Fact]
      public void Direct_Task_Transform_With_Error_In_Kickoff_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);

        //Act
        var hr = target.ProcessCommandPossibleAsync<int>(namespaceCtx, 1, (cts) => throw new Exception(), UncalledCallback<int>);

        //Assert
        Assert.Equal(HRESULT.E_INVALIDARG, hr);
      }

      [ExcludeFromCodeCoverage]
      private static HRESULT UncalledCallback<TInput>(TInput value)
      {
        throw new Exception("Test failed if callback invoked");
      }

      [Fact]
      public void Direct_Task_Transform_With_Error_In_Transform_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);

        //Act
        var hr = target.ProcessCommandPossibleAsync<int>(namespaceCtx, 1, (cts) => Task.FromResult(1), (value) => throw new Exception());

        //Assert
        Assert.Equal(HRESULT.E_INVALIDARG, hr);
      }

      [Fact]
      public void Direct_Task_With_NotificationMask_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);
        var parms = new PRJ_NOTIFICATION_PARAMETERS();

        //Act
        var hr = target.ProcessCommandPossibleAsyncWithNotificationMask(namespaceCtx, 1, (cts) => throw new Exception(), parms);

        //Assert
        Assert.Equal(HRESULT.E_INVALIDARG, hr);
      }

      [Fact]
      public void Indirect_Task_No_Transform_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (Func<CancellationTokenSource, Task>)((cts) => Task.FromException(new Exception())));

        //Assert
        Assert.Equal(HRESULT.E_INVALIDARG, hr);
      }

      [Fact]
      public void Indirect_Task_Transform_With_Error_In_Kickoff_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);
        var tcs = new TaskCompletionSource<int>();
        tcs.SetException(new Exception());

        //Act
        var hr = target.ProcessCommandPossibleAsync<int>(namespaceCtx, 1, (cts) => tcs.Task, UncalledCallback<int>);

        //Assert
        Assert.Equal(HRESULT.E_INVALIDARG, hr);
      }

      [Fact]
      public void Indirect_Task_With_NotificationMask_Is_Error()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);
        var parms = new PRJ_NOTIFICATION_PARAMETERS();
        var tcs = new TaskCompletionSource<NotificationRequired>();
        tcs.SetException(new Exception());

        //Act
        var hr = target.ProcessCommandPossibleAsyncWithNotificationMask(namespaceCtx, 1, (cts) => tcs.Task, parms);

        //Assert
        Assert.Equal(HRESULT.E_INVALIDARG, hr);
      }
    }
  }
}
