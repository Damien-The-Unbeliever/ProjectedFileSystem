using Moq;
using ProjectedFileSystem.Core.FileSystem;
using ProjectedFileSystem.Core.Impl;
using ProjectedFileSystem.Core.Native;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectedFileSystem.Core.Tests.AsyncManagerTests
{
  public partial class Sync
  {
    public class Success
    {
      [Fact]
      public void Task_No_Transform_Is_S_OK()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (cts) => Task.CompletedTask);

        //Assert
        Assert.Equal(HRESULT.S_OK, hr);
      }
      [Fact]
      public void ValueTask_No_Transform_Is_S_OK()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (cts) => new ValueTask());

        //Assert
        Assert.Equal(HRESULT.S_OK, hr);
      }
      [Fact]
      public void Task_With_Transform_Is_S_OK()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (cts) => Task.FromResult(4), (value) => HRESULT.S_OK);

        //Assert
        Assert.Equal(HRESULT.S_OK, hr);
      }
      [Fact]
      public void Task_With_Transform_Is_Other_Result()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);

        //Act
        var hr = target.ProcessCommandPossibleAsync(namespaceCtx, 1, (cts) => Task.FromResult(4), (value) => HRESULT.S_FALSE);

        //Assert
        Assert.Equal(HRESULT.S_FALSE, hr);
      }

      [Fact]
      public void Task_With_NotificationMask_Is_S_OK()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var target = new AsyncManager(functions.Object);
        var namespaceCtx = new IntPtr(37);
        var parms = new PRJ_NOTIFICATION_PARAMETERS();

        //Act
        var hr = target.ProcessCommandPossibleAsyncWithNotificationMask(namespaceCtx, 1, (cts) => Task.FromResult(NotificationRequired.FileCreated), parms);

        //Assert
        Assert.Equal(HRESULT.S_OK, hr);
        Assert.Equal(PRJ_NOTIFY_TYPES.PRJ_NOTIFY_NEW_FILE_CREATED, parms.NotificationMask);
      }
    }
  }
}
