using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ProjectedFileSystem.Core.Native.Callbacks;

namespace ProjectedFileSystem.Core.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal struct PRJ_CALLBACKS
  {
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public PRJ_START_DIRECTORY_ENUMERATION_CB StartDirectoryEnumerationCallback;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public PRJ_END_DIRECTORY_ENUMERATION_CB EndDirectoryEnumerationCallback;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public PRJ_GET_DIRECTORY_ENUMERATION_CB GetDirectoryEnumerationCallback;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public PRJ_GET_PLACEHOLDER_INFO_CB GetPlaceholderInfoCallback;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public PRJ_GET_FILE_DATA_CB GetFileDataCallback;

    [MarshalAs(UnmanagedType.FunctionPtr)]
    public PRJ_QUERY_FILE_NAME_CB QueryFileNameCallback;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public PRJ_NOTIFICATION_CB NotificationCallback;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public PRJ_CANCEL_COMMAND_CB CancelCommandCallback;
  }
}
