using ProjectedFileSystem.Core.Impl;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace ProjectedFileSystem.Core.Tests
{
  public class NativeBufferTests
  {
    [StructLayout(LayoutKind.Sequential)]
    private class SimpleClass
    {
      public int First;
      public int Second;
    }

    [Fact]
    public void Can_Roundtrip_Simple_Structure()
    {
      //Arrange
      var input = new SimpleClass { First = 1234567, Second = 987654 };

      //Act
      using (var nb = new NativeBuffer<SimpleClass>(input))
      {
        var output = NativeBuffer<SimpleClass>.Recover(nb.Buffer);

        //Assert
        Assert.NotSame(input, output);
        Assert.Equal(input.First, output.First);
        Assert.Equal(input.Second, output.Second);
      }
    }

    [Fact]
    public void Simple_Structure_Expected_Layout()
    {
      unchecked
      {
        //Arrange
        var input = new SimpleClass { First = 0x12345678, Second = (int)0x99999999 };

        //Act
        using(var nb = new NativeBuffer<SimpleClass>(input))
        {
          var buffer = nb.Buffer;
          var first = Marshal.ReadInt32(buffer);
          var second = Marshal.ReadInt32(buffer + 4);

          //Assert
          Assert.Equal(input.First, first);
          Assert.Equal(input.Second, second);
        }
      }
    }

    [Fact]
    public void Simple_Structure_With_Extended_Data_Readable()
    {
      //Arrange
      var input = new SimpleClass();
      var additional = new byte[] { 4, 99, 3 };

      //Act
      using(var nb = new NativeBuffer<SimpleClass>(input,additional))
      {
        var buffer = nb.Buffer;
        var b1 = Marshal.ReadByte(buffer + 8);
        var b2 = Marshal.ReadByte(buffer + 9);
        var b3 = Marshal.ReadByte(buffer + 10);

        //Assert
        Assert.Equal(4, b1);
        Assert.Equal(99, b2);
        Assert.Equal(3, b3);
      }
    }
  }
}

