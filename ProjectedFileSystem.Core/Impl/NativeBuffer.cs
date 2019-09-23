using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ProjectedFileSystem.Core.Impl
{
  internal sealed class NativeBuffer<T> : IDisposable
  {
    private readonly IntPtr _buffer;
    private readonly int _totalSize;
    private readonly int _structureSize;
    private T _value;
    private bool _disposed = false;
    private static readonly byte[] _emptyAdditional = new byte[] { };
    public NativeBuffer(T value, byte[] additionalData)
    {
      _value = value;
      _structureSize = Marshal.SizeOf<T>();
      _totalSize = _structureSize + additionalData.Length;
      _buffer = Marshal.AllocHGlobal(_totalSize);
      Marshal.StructureToPtr<T>(value, _buffer, false);
      if (additionalData.Length > 0)
      {
        Marshal.Copy(additionalData, 0, _buffer + _structureSize, additionalData.Length);
      }
    }
    public NativeBuffer(T value) : this(value, _emptyAdditional)
    {

    }
    public (int structureSize, int totalSize) Sizes => (_structureSize, _totalSize);
    public IntPtr Buffer
    {
      get
      {
        if (_disposed) throw new ObjectDisposedException(nameof(NativeBuffer<T>));
        return _buffer;
      }
    }
    public void Dispose()
    {
      if (_disposed) return;
      Marshal.FreeHGlobal(_buffer);
      GC.KeepAlive(_value);
      _value = default;
      _disposed = true;
    }
    [ExcludeFromCodeCoverage]
    ~NativeBuffer()
    {
      Dispose();
    }

    public static T Recover(IntPtr buffer)
    {
      return Marshal.PtrToStructure<T>(buffer);
    }
  }
}
