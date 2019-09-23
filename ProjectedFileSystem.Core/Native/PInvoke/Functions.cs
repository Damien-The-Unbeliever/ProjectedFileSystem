using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native.PInvoke
{
  internal static class Functions
  {
    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern HRESULT PrjStartVirtualizing(
      [MarshalAs(UnmanagedType.LPWStr)] String virtualizationRootPath,
      IntPtr callbacks, /* PRJ_CALLBACKS */
      IntPtr instanceContext,
      [MarshalAs(UnmanagedType.LPStruct)] PRJ_STARTVIRTUALIZING_OPTIONS options,
      out IntPtr namespaceVirtualizationContext
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern void PrjStopVirtualizing(
      IntPtr namespaceVirtualizationContext
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern HRESULT PrjClearNegativePathCache(
      IntPtr namespaceVirtualizationContext,
      out int totalEntryNumber
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern HRESULT PrjGetVirtualizationInstanceInfo(
      IntPtr namespaceVirtualizationContext,
      ref PRJ_VIRTUALIZATION_INSTANCE_INFO virtualizationInstanceInfo
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern HRESULT PrjMarkDirectoryAsPlaceholder(
      [MarshalAs(UnmanagedType.LPWStr)] String rootPathName,
      [MarshalAs(UnmanagedType.LPWStr)] String targetPathName,
      [MarshalAs(UnmanagedType.LPStruct)] PRJ_PLACEHOLDER_VERSION_INFO versionInfo,
      [MarshalAs(UnmanagedType.LPStruct)] Guid virtualizationInstanceID
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern HRESULT PrjWritePlaceholderInfo(
      IntPtr namespaceVirtualizationContext,
      [MarshalAs(UnmanagedType.LPWStr)] String destinationFileName,
      IntPtr placeholderInfo,
      int placeholderInfoSize
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern HRESULT PrjUpdateFileIfNeeded(
      IntPtr namespaceVirtualizationContext,
      [MarshalAs(UnmanagedType.LPWStr)] String destinationFileName,
      [MarshalAs(UnmanagedType.LPStruct)] PRJ_PLACEHOLDER_INFO placeholderInfo,
      int placeholderInfoSize,
      PRJ_UPDATE_TYPES updateFlags,
      out PRJ_UPDATE_FAILURE_CAUSES failureReason
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern HRESULT PrjDeleteFile(
      IntPtr namespaceVirtualizationContext,
      [MarshalAs(UnmanagedType.LPWStr)] String destinationFileName,
      PRJ_UPDATE_TYPES updateFlags,
      out PRJ_UPDATE_FAILURE_CAUSES failureReason
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern HRESULT PrjWriteFileData(
      IntPtr namespaceVirtualizationContext,
      [MarshalAs(UnmanagedType.LPStruct)] Guid dataStreamId,
      IntPtr buffer,
      ulong byteOffset,
      uint length
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern HRESULT PrjGetOnDiskFileState(
      [MarshalAs(UnmanagedType.LPWStr)] String destinationFileName,
      out PRJ_FILE_STATE fileState
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern AlignedBufferHandle PrjAllocateAlignedBuffer(
      IntPtr namespaceVirtualizationContext,
      UIntPtr size
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern void PrjFreeAlignedBuffer(
      IntPtr buffer
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern HRESULT PrjCompleteCommand(
      IntPtr namespaceVirtualizationContext,
      int commandId,
      HRESULT completionResult,
      IntPtr args
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern HRESULT PrjFillDirEntryBuffer(
    [MarshalAs(UnmanagedType.LPWStr)] String fileName,
      PRJ_FILE_BASIC_INFO fileBasicInfo,
      IntPtr dirEntryBufferHandle
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern bool PrjFileNameMatch(
      [MarshalAs(UnmanagedType.LPWStr)] String fileNameToCheck,
      [MarshalAs(UnmanagedType.LPWStr)] String pattern
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern int PrjFileNameCompare(
      [MarshalAs(UnmanagedType.LPWStr)] String fileName1,
      [MarshalAs(UnmanagedType.LPWStr)] String fileName2
    );

    [DllImport("ProjectedFSLib.dll", CallingConvention = CallingConvention.StdCall)]
    internal static extern bool PrjDoesNameContainWildCards(
      [MarshalAs(UnmanagedType.LPWStr)] String fileName
    );
  }
}
