using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectedFileSystem.Core.Native
{
  public class AlignedBufferHandle : SafeHandleMinusOneIsInvalid
  {
    private readonly Functions _outbound;
    internal AlignedBufferHandle(Functions outboundFunctions) : base(true)
    {
      _outbound = outboundFunctions;
    }
    protected override bool ReleaseHandle()
    {
      _outbound.PrjFreeAlignedBuffer(handle);
      return true;
    }

    internal static AlignedBufferHandle FromIntPtr(Functions outboundFunctions, IntPtr value)
    {
      var result = new AlignedBufferHandle(outboundFunctions);
      result.SetHandle(value);
      return result;
    }
  }
}
