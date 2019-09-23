using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectedFileSystem.Core.Native
{
  internal abstract partial class Functions
  {
    internal abstract HRESULT PrjStartVirtualizing(
      String virtualizationRootPath,
      IntPtr callbacks,
      IntPtr instanceContext,
      PRJ_STARTVIRTUALIZING_OPTIONS options,
      out IntPtr namespaceVirtualizationContext
    );
    internal abstract void PrjStopVirtualizing(
      IntPtr namespaceVirtualizationContext
    );
    internal abstract HRESULT PrjClearNegativePathCache(
      IntPtr namespaceVirtualizationContext,
      out int totalEntryNumber
    );
    internal abstract HRESULT PrjGetVirtualizationInstanceInfo(
      IntPtr namespaceVirtualizationContext,
      ref PRJ_VIRTUALIZATION_INSTANCE_INFO virtualizationInstanceInfo
    );
    internal abstract HRESULT PrjMarkDirectoryAsPlaceholder(
       String rootPathName,
       String targetPathName,
       PRJ_PLACEHOLDER_VERSION_INFO versionInfo,
       Guid virtualizationInstanceID
    );
    internal abstract HRESULT PrjWritePlaceholderInfo(
      IntPtr namespaceVirtualizationContext,
       String destinationFileName,
      IntPtr placeholderInfo,
      int placeholderInfoSize
    );
    internal abstract HRESULT PrjUpdateFileIfNeeded(
      IntPtr namespaceVirtualizationContext,
      String destinationFileName,
      PRJ_PLACEHOLDER_INFO placeholderInfo,
      int placeholderInfoSize,
      PRJ_UPDATE_TYPES updateFlags,
      out PRJ_UPDATE_FAILURE_CAUSES failureReason
    );
    internal abstract HRESULT PrjDeleteFile(
      IntPtr namespaceVirtualizationContext,
      String destinationFileName,
      PRJ_UPDATE_TYPES updateFlags,
      out PRJ_UPDATE_FAILURE_CAUSES failureReason
    );
    internal abstract HRESULT PrjWriteFileData(
      IntPtr namespaceVirtualizationContext,
      Guid dataStreamId,
      IntPtr buffer,
      ulong byteOffset,
      uint length
    );
    internal abstract HRESULT PrjGetOnDiskFileState(
      String destinationFileName,
      out PRJ_FILE_STATE fileState
    );
    internal abstract AlignedBufferHandle PrjAllocateAlignedBuffer(
      IntPtr namespaceVirtualizationContext,
      UIntPtr size
    );
    internal abstract void PrjFreeAlignedBuffer(
      IntPtr buffer
    );
    internal abstract HRESULT PrjCompleteCommand(
      IntPtr namespaceVirtualizationContext,
      int commandId,
      HRESULT completionResult,
      IntPtr args
    );
    internal abstract HRESULT PrjFillDirEntryBuffer(
     String fileName,
      PRJ_FILE_BASIC_INFO fileBasicInfo,
      IntPtr dirEntryBufferHandle
    );
    internal abstract bool PrjFileNameMatch(
       String fileNameToCheck,
       String pattern
    );
    internal abstract int PrjFileNameCompare(
       String fileName1,
       String fileName2
    );
    internal abstract bool PrjDoesNameContainWildCards(
       String fileName
    );
  }
}
