using System;
using System.Collections.Generic;
using System.Text;


namespace ProjectedFileSystem.Core.Native
{
  internal abstract partial class Functions
  {
    internal class DefaultBinding : Functions
    {
      internal override AlignedBufferHandle PrjAllocateAlignedBuffer(IntPtr namespaceVirtualizationContext, UIntPtr size)
      {
        return PInvoke.Functions.PrjAllocateAlignedBuffer(namespaceVirtualizationContext, size);
      }

      internal override HRESULT PrjClearNegativePathCache(IntPtr namespaceVirtualizationContext, out int totalEntryNumber)
      {
        return PInvoke.Functions.PrjClearNegativePathCache(namespaceVirtualizationContext, out totalEntryNumber);
      }

      internal override HRESULT PrjCompleteCommand(IntPtr namespaceVirtualizationContext, int commandId, HRESULT completionResult, IntPtr args)
      {
        return PInvoke.Functions.PrjCompleteCommand(namespaceVirtualizationContext, commandId, completionResult, args);
      }

      internal override HRESULT PrjDeleteFile(IntPtr namespaceVirtualizationContext, string destinationFileName, PRJ_UPDATE_TYPES updateFlags, out PRJ_UPDATE_FAILURE_CAUSES failureReason)
      {
        return PInvoke.Functions.PrjDeleteFile(namespaceVirtualizationContext, destinationFileName, updateFlags, out failureReason);
      }

      internal override bool PrjDoesNameContainWildCards(string fileName)
      {
        return PInvoke.Functions.PrjDoesNameContainWildCards(fileName);
      }

      internal override int PrjFileNameCompare(string fileName1, string fileName2)
      {
        return PInvoke.Functions.PrjFileNameCompare(fileName1, fileName2);
      }

      internal override bool PrjFileNameMatch(string fileNameToCheck, string pattern)
      {
        return PInvoke.Functions.PrjFileNameMatch(fileNameToCheck, pattern);
      }

      internal override HRESULT PrjFillDirEntryBuffer(string fileName, PRJ_FILE_BASIC_INFO fileBasicInfo, IntPtr dirEntryBufferHandle)
      {
        return PInvoke.Functions.PrjFillDirEntryBuffer(fileName, fileBasicInfo, dirEntryBufferHandle);
      }

      internal override void PrjFreeAlignedBuffer(IntPtr buffer)
      {
        PInvoke.Functions.PrjFreeAlignedBuffer(buffer);
      }

      internal override HRESULT PrjGetOnDiskFileState(string destinationFileName, out PRJ_FILE_STATE fileState)
      {
        return PInvoke.Functions.PrjGetOnDiskFileState(destinationFileName, out fileState);
      }

      internal override HRESULT PrjGetVirtualizationInstanceInfo(IntPtr namespaceVirtualizationContext, ref PRJ_VIRTUALIZATION_INSTANCE_INFO virtualizationInstanceInfo)
      {
        return PInvoke.Functions.PrjGetVirtualizationInstanceInfo(namespaceVirtualizationContext, ref virtualizationInstanceInfo);
      }

      internal override HRESULT PrjMarkDirectoryAsPlaceholder(string rootPathName, string targetPathName, PRJ_PLACEHOLDER_VERSION_INFO versionInfo, Guid virtualizationInstanceID)
      {
        return PInvoke.Functions.PrjMarkDirectoryAsPlaceholder(rootPathName, targetPathName, versionInfo, virtualizationInstanceID);
      }

      internal override HRESULT PrjStartVirtualizing(string virtualizationRootPath, IntPtr callbacks, IntPtr instanceContext, PRJ_STARTVIRTUALIZING_OPTIONS options, out IntPtr namespaceVirtualizationContext)
      {
        return PInvoke.Functions.PrjStartVirtualizing(virtualizationRootPath, callbacks, instanceContext, options, out namespaceVirtualizationContext);
      }

      internal override void PrjStopVirtualizing(IntPtr namespaceVirtualizationContext)
      {
        PInvoke.Functions.PrjStopVirtualizing(namespaceVirtualizationContext);
      }

      internal override HRESULT PrjUpdateFileIfNeeded(IntPtr namespaceVirtualizationContext, string destinationFileName, PRJ_PLACEHOLDER_INFO placeholderInfo, int placeholderInfoSize, PRJ_UPDATE_TYPES updateFlags, out PRJ_UPDATE_FAILURE_CAUSES failureReason)
      {
        return PInvoke.Functions.PrjUpdateFileIfNeeded(namespaceVirtualizationContext, destinationFileName, placeholderInfo, placeholderInfoSize, updateFlags, out failureReason);
      }

      internal override HRESULT PrjWriteFileData(IntPtr namespaceVirtualizationContext, Guid dataStreamId, IntPtr buffer, ulong byteOffset, uint length)
      {
        return PInvoke.Functions.PrjWriteFileData(namespaceVirtualizationContext, dataStreamId, buffer, byteOffset, length);
      }

      internal override HRESULT PrjWritePlaceholderInfo(IntPtr namespaceVirtualizationContext, string destinationFileName, IntPtr placeholderInfo, int placeholderInfoSize)
      {
        return PInvoke.Functions.PrjWritePlaceholderInfo(namespaceVirtualizationContext, destinationFileName, placeholderInfo, placeholderInfoSize);
      }
    }
  }
}
