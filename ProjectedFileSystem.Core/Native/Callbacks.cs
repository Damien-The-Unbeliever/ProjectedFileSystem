using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.Native
{
  internal static class Callbacks
  {
    internal delegate HRESULT PRJ_START_DIRECTORY_ENUMERATION_CB(
      PRJ_CALLBACK_DATA callbackData,
      [MarshalAs(UnmanagedType.LPStruct)]
      Guid enumerationId
    );

    internal delegate HRESULT PRJ_GET_DIRECTORY_ENUMERATION_CB(
      PRJ_CALLBACK_DATA callbackData,
      [MarshalAs(UnmanagedType.LPStruct)]
      Guid enumerationId,
      [MarshalAs(UnmanagedType.LPWStr)]
      String searchExpression,
      IntPtr dirEntryBufferHandle
    );

    internal delegate HRESULT PRJ_END_DIRECTORY_ENUMERATION_CB(
      PRJ_CALLBACK_DATA callbackData,
      [MarshalAs(UnmanagedType.LPStruct)]
      Guid enumerationId
    );

    internal delegate HRESULT PRJ_GET_PLACEHOLDER_INFO_CB(
      PRJ_CALLBACK_DATA callbackData
    );

    internal delegate HRESULT PRJ_GET_FILE_DATA_CB(
      PRJ_CALLBACK_DATA callbackData,
      ulong byteOffset,
      uint length
    );

    internal delegate HRESULT PRJ_QUERY_FILE_NAME_CB(
      PRJ_CALLBACK_DATA callbackData
    );

    internal delegate HRESULT PRJ_NOTIFICATION_CB(
      PRJ_CALLBACK_DATA callbackData,
      bool isDirectory,
      PRJ_NOTIFICATION notification,
      [MarshalAs(UnmanagedType.LPWStr)]
      String destinationFileName,
      ref PRJ_NOTIFICATION_PARAMETERS operationParameters
    );

    internal delegate void PRJ_CANCEL_COMMAND_CB(
      PRJ_CALLBACK_DATA callbackData
    );
  }
}
