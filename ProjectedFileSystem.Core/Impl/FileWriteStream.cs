using ProjectedFileSystem.Core.Native;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ProjectedFileSystem.Core.Impl
{
  internal sealed class FileWriteStream : Stream
  {
    private readonly AlignedBufferHandle _currentBuffer;
    private readonly int _allocationSize;
    private readonly IntPtr _namespaceVirtualizationContext;
    private readonly Guid _dataStreamId;
    private readonly Functions _outboundFunctions;
    private ulong _currentFileOffset;
    private int _currentBufferOffset;
    private int _currentFlushOffset;
    public FileWriteStream(ulong fileOffset, int allocationSize, IntPtr namespaceVirtualizationContext, Guid dataStreamId, Functions outboundFunctions)
    {
      if (fileOffset % (ulong)allocationSize != 0) throw new NotSupportedException();
      _currentFileOffset = fileOffset;
      _allocationSize = allocationSize;
      _namespaceVirtualizationContext = namespaceVirtualizationContext;
      _dataStreamId = dataStreamId;
      _outboundFunctions = outboundFunctions;
      _currentBuffer = _outboundFunctions.PrjAllocateAlignedBuffer(namespaceVirtualizationContext,(UIntPtr)(uint)allocationSize);
      _currentBufferOffset = 0;
      _currentFileOffset = 0;
    }
    public override bool CanRead => false;

    public override bool CanSeek => false;

    public override bool CanWrite => true;


    public override void Write(byte[] buffer, int offset, int count)
    {
      while (count > 0)
      {
        var sizeInCurrentBuffer = Math.Min(count, _allocationSize - _currentBufferOffset);
        Marshal.Copy(buffer, offset, _currentBuffer.DangerousGetHandle() + _currentBufferOffset, sizeInCurrentBuffer);
        _currentBufferOffset += sizeInCurrentBuffer;
        if (_currentBufferOffset == _allocationSize)
        {
          Flush();
        }
        offset += sizeInCurrentBuffer;
        count -= sizeInCurrentBuffer;
      }
    }
    public override void Flush()
    {
      //Write _currentBufferOffset - _currentFlushOffset bytes 
      //To the file at _currentFileOffset + _currentFlushOffset
      //Make _currentFlushOffset = _currentBufferOffset
      //If both are at end, reset to start of buffer
      var bytesToWrite = _currentBufferOffset - _currentFlushOffset;
      if (bytesToWrite < 0) throw new NotSupportedException();
      if (bytesToWrite == 0) return;
      if (_outboundFunctions.PrjWriteFileData(
        _namespaceVirtualizationContext,
        _dataStreamId,
        _currentBuffer.DangerousGetHandle() + _currentFlushOffset,
        (_currentFileOffset + (ulong)_currentFlushOffset),
        (uint)bytesToWrite) != HRESULT.S_OK) throw new NotSupportedException();
      _currentFileOffset += (ulong)bytesToWrite;
      if (_currentBufferOffset == _allocationSize)
      {
        _currentBufferOffset = 0;
        _currentFlushOffset = 0;
      }
      else
      {
        _currentFlushOffset += bytesToWrite;
      }
    }
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        Flush();
        _currentBuffer.Dispose();
      }
      base.Dispose(disposing);
    }
    #region "Unseakable and Unreadable"
    public override long Length => throw new NotSupportedException();
    public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
    public override int Read(byte[] buffer, int offset, int count)
    {
      throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
      throw new NotSupportedException();
    }

    #endregion
  }
}
