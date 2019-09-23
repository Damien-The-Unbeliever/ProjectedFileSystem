using Moq;
using ProjectedFileSystem.Core.Impl;
using ProjectedFileSystem.Core.Native;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace ProjectedFileSystem.Core.Tests.AsyncManagerTests
{
  public partial class Async
  {
    public class Cancellation
    {
      [Fact]
      public void BasicUsageSound()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        var target = new AsyncManager(functions.Object);

        //Act
        var cts = target.GetCancellationTokenForCommand(99);
        bool cancellationCalled = false;
        cts.Token.Register(() =>
        {
          cancellationCalled = true;
        });
        target.ProcessCancelCommand(99);

        //Assert
        Assert.True(cancellationCalled);
      }

      [Fact]
      public void AllCancelledOnDispose()
      {
        //Arrange
        var functions = new Mock<Functions>();
        var namespaceCtx = new IntPtr(37);
        var target = new AsyncManager(functions.Object);

        //Act
        int cancellationsCalled = 0;
        var cts1 = target.GetCancellationTokenForCommand(99);
        cts1.Token.Register(() =>
        {
          Interlocked.Increment(ref cancellationsCalled);
        });
        var cts2 = target.GetCancellationTokenForCommand(99);
        cts2.Token.Register(() =>
        {
          Interlocked.Increment(ref cancellationsCalled);
        });
        var cts3 = target.GetCancellationTokenForCommand(99);
        cts3.Token.Register(() =>
        {
          Interlocked.Increment(ref cancellationsCalled);
        });
        target.Dispose();

        //Assert
        Assert.Equal(3, cancellationsCalled);
      }
    }
  }
}
