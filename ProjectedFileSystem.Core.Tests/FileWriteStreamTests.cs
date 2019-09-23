using Moq;
using System;
using Xunit;
using ProjectedFileSystem.Core.Native;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ProjectedFileSystem.Core.Impl;
using System.Linq;

namespace ProjectedFileSystem.Core.Tests
{
  public class FileWriteStreamTests
  {
    [Fact]
    public void Unused_Stream_Writes_No_Data()
    {
      //Arrange
      var functions = new Mock<Functions>();
      var writeCalls = new List<(long, int)>();
      var namespaceVirt = (IntPtr)7;
      var dataStreamId = Guid.NewGuid();
      var allocSize = (2 << 12);
      IntPtr buffer = Marshal.AllocHGlobal(allocSize);
      var aligned = AlignedBufferHandle.FromIntPtr(functions.Object, buffer);
      functions.Setup(f => f.PrjAllocateAlignedBuffer(namespaceVirt, (UIntPtr)allocSize)).Returns(aligned);
      functions.Setup(f => f.PrjFreeAlignedBuffer(buffer));

      //Act
      var stream = new FileWriteStream(0, allocSize, namespaceVirt, dataStreamId, functions.Object);
      stream.Dispose();

      //Assert
      functions.VerifyAll();
      functions.VerifyNoOtherCalls();
    }

    [Fact]
    public void Small_Write_Writes_Correct_Size()
    {
      //Arrange
      var functions = new Mock<Functions>();
      var writeCalls = new List<(IntPtr bPointer, ulong offset, uint length)>();
      var namespaceVirt = (IntPtr)7;
      var dataStreamId = Guid.NewGuid();
      var allocSize = (2 << 12);
      IntPtr buffer = Marshal.AllocHGlobal(allocSize);
      var aligned = AlignedBufferHandle.FromIntPtr(functions.Object, buffer);
      IntPtr bufferEnd = buffer + allocSize - 1;
      functions.Setup(f => f.PrjAllocateAlignedBuffer(namespaceVirt, (UIntPtr)allocSize)).Returns(aligned);
      functions.Setup(f => f.PrjFreeAlignedBuffer(buffer));
      functions.Setup(f => f.PrjWriteFileData(namespaceVirt, dataStreamId, It.IsAny<IntPtr>(), It.IsAny<ulong>(), It.IsAny<uint>()))
        .Returns(HRESULT.S_OK)
        .Callback((IntPtr ctx, Guid stream, IntPtr newBuffer, ulong offset, uint length) => writeCalls.Add((newBuffer, offset, length)));
      int totalWriteSize = 200;
      //Act
      var stream = new FileWriteStream(0, allocSize, namespaceVirt, dataStreamId, functions.Object);
      stream.Write(Enumerable.Repeat<byte>(5, totalWriteSize).ToArray());
      stream.Dispose();

      //Assert
      AssertWriteSizes(writeCalls, buffer, bufferEnd, totalWriteSize);
    }

    [Fact]
    public void Large_Write_Writes_Correct_Size()
    {
      //Arrange
      var functions = new Mock<Functions>();
      var writeCalls = new List<(IntPtr bPointer, ulong offset, uint length)>();
      var namespaceVirt = (IntPtr)7;
      var dataStreamId = Guid.NewGuid();
      var allocSize = (2 << 12);
      IntPtr buffer = Marshal.AllocHGlobal(allocSize);
      var aligned = AlignedBufferHandle.FromIntPtr(functions.Object, buffer);
      IntPtr bufferEnd = buffer + allocSize - 1;
      functions.Setup(f => f.PrjAllocateAlignedBuffer(namespaceVirt, (UIntPtr)allocSize)).Returns(aligned);
      functions.Setup(f => f.PrjFreeAlignedBuffer(buffer));
      functions.Setup(f => f.PrjWriteFileData(namespaceVirt, dataStreamId, It.IsAny<IntPtr>(), It.IsAny<ulong>(), It.IsAny<uint>()))
        .Returns(HRESULT.S_OK)
        .Callback((IntPtr ctx, Guid stream, IntPtr newBuffer, ulong offset, uint length) => writeCalls.Add((newBuffer, offset, length)));
      int totalWriteSize = 16000;
      //Act
      var stream = new FileWriteStream(0, allocSize, namespaceVirt, dataStreamId, functions.Object);
      stream.Write(Enumerable.Repeat<byte>(5, totalWriteSize).ToArray());
      stream.Dispose();

      //Assert
      AssertWriteSizes(writeCalls, buffer, bufferEnd, totalWriteSize);
    }

    [Fact]
    public void No_Bold_Claims()
    {
      //Arrange
      var functions = new Mock<Functions>();
      var writeCalls = new List<(long, int)>();
      var namespaceVirt = (IntPtr)7;
      var dataStreamId = Guid.NewGuid();
      var allocSize = (2 << 12);
      IntPtr buffer = Marshal.AllocHGlobal(allocSize);
      var aligned = AlignedBufferHandle.FromIntPtr(functions.Object, buffer);

      functions.Setup(f => f.PrjAllocateAlignedBuffer(namespaceVirt, (UIntPtr)allocSize)).Returns(aligned);

      //Act
      var stream = new FileWriteStream(0, allocSize, namespaceVirt, dataStreamId, functions.Object);

      //Assert
      var tempBuffer = new byte[200];
      Assert.False(stream.CanRead);
      Assert.False(stream.CanSeek);
      Assert.True(stream.CanWrite);
      Assert.Throws<NotSupportedException>(() => stream.Read(tempBuffer, 0, tempBuffer.Length));
      Assert.Throws<NotSupportedException>(() => stream.Seek(0, System.IO.SeekOrigin.Begin));
      Assert.Throws<NotSupportedException>(() => stream.SetLength(0));
      Assert.Throws<NotSupportedException>(() => stream.Position = 10);
      Assert.Throws<NotSupportedException>(() => { var item = stream.Position; });
      Assert.Throws<NotSupportedException>(() => { var length = stream.Length; });


    }

    private static void AssertWriteSizes(List<(IntPtr bPointer, ulong offset, uint length)> writeCalls, IntPtr buffer, IntPtr bufferEnd, int totalWriteSize)
    {
      Assert.NotEmpty(writeCalls);
      var (bPointer, offset, length) = writeCalls[0];
      Assert.Equal((ulong)0, offset);
      Assert.InRange((ulong)bPointer, (ulong)buffer, (ulong)bufferEnd);
      Assert.InRange((ulong)(bPointer + (int)length - 1), (ulong)buffer, (ulong)bufferEnd);
      var lastWritePosition = offset + length;
      foreach (var furtherWrite in writeCalls.Skip(1))
      {
        Assert.Equal(lastWritePosition, furtherWrite.offset);
        Assert.InRange((ulong)furtherWrite.bPointer, (ulong)buffer, (ulong)bufferEnd);
        Assert.InRange((ulong)(furtherWrite.bPointer + (int)furtherWrite.length - 1), (ulong)buffer, (ulong)bufferEnd);
        lastWritePosition += furtherWrite.length;
      }
      Assert.Equal((ulong)totalWriteSize, lastWritePosition);
    }
  }
}
