using ProjectedFileSystem.Core.Interfaces;
using ProjectedFileSystem.Core.Native;
using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using static ProjectedFileSystem.Core.Native.Callbacks;

namespace ProjectedFileSystem.Core.Impl
{
  internal sealed partial class RunningInstance : CriticalFinalizerObject, IRunningInstance
  {
    private readonly IntPtr _namespaceVirtualizationContext;
    private readonly NativeBuffer<PRJ_CALLBACKS> _callbacks;
    private readonly PRJ_VIRTUALIZATION_INSTANCE_INFO _virtualizationInfo;
    private readonly AsyncManager _asyncOperations;
    private readonly Functions _outboundFunctions;
    internal RunningInstance(IFileSystem fileSystem, InstanceOptions options, IRunnableInstance instanceOf, Functions outboundFunctions)
    {
      FileSystem = fileSystem;
      InstanceOf = instanceOf;
      _outboundFunctions = outboundFunctions;
      _asyncOperations = new AsyncManager(_outboundFunctions);
      var callbacks = new PRJ_CALLBACKS
      {
        CancelCommandCallback = ProcessCancelCommand,
        QueryFileNameCallback = fileSystem is ISeekableFileSystem ? ProcessQueryFileName : (PRJ_QUERY_FILE_NAME_CB)null,
        StartDirectoryEnumerationCallback = ProcessStartEnumeration,
        EndDirectoryEnumerationCallback = ProcessEndEnumeration,
        GetDirectoryEnumerationCallback = ProcessEnumerationStep,
        GetPlaceholderInfoCallback = ProcessPlaceholderRequest,
        GetFileDataCallback = ProcessDataRequest
      };
      PRJ_STARTVIRTUALIZING_OPTIONS optionsNative = CreateOptions(fileSystem, options, callbacks);
      _callbacks = new NativeBuffer<PRJ_CALLBACKS>(callbacks);

      if (_outboundFunctions.PrjStartVirtualizing(
        instanceOf.RootPath,
        _callbacks.Buffer,
        IntPtr.Zero,
        optionsNative,
        out _namespaceVirtualizationContext) != HRESULT.S_OK) throw new Exception("Failed to start up");
      if (_outboundFunctions.PrjGetVirtualizationInstanceInfo(_namespaceVirtualizationContext, ref _virtualizationInfo) != HRESULT.S_OK) throw new Exception("Failed to start up");
    }

    public IFileSystem FileSystem { get; }

    public IRunnableInstance InstanceOf { get; }

    public void Dispose()
    {
      Dispose(true);
    }

    private void Dispose(bool disposing)
    {
      _outboundFunctions.PrjStopVirtualizing(_namespaceVirtualizationContext);
      if (disposing)
      {
        _callbacks.Dispose();
        _asyncOperations.Dispose();
      }
    }

    public void Shutdown()
    {
      Dispose();
    }
    ~RunningInstance()
    {
      Dispose(false);
    }

    public void ProcessCancelCommand(PRJ_CALLBACK_DATA callbackData)
    {
      _asyncOperations.ProcessCancelCommand(callbackData.CommandId);
    }
    private PRJ_STARTVIRTUALIZING_OPTIONS CreateOptions(IFileSystem fileSystem, InstanceOptions options, PRJ_CALLBACKS callbacks)
    {
      var initialNotifications = Enumerable.Empty<PRJ_NOTIFICATION_MAPPING>();
      if (fileSystem is INotifiableFileSystem notifiable)
      {
        callbacks.NotificationCallback = ProcessNotification;
        initialNotifications = notifiable.StartingNotifications().Select(n => new PRJ_NOTIFICATION_MAPPING { NotificationRoot = n.Path, NotificationBitMask = (PRJ_NOTIFY_TYPES)(int)n.Notifications });
      }

      var optionsNative = new PRJ_STARTVIRTUALIZING_OPTIONS
      {
        PoolThreadCount = options.PoolThreadCount,
        ConcurrentThreadCount = options.ConcurrentThreadCount,
        Flags = options.NegativePathCache ? PRJ_STARTVIRTUALIZING_FLAGS.PRJ_FLAG_USE_NEGATIVE_PATH_CACHE : PRJ_STARTVIRTUALIZING_FLAGS.PRJ_FLAG_NONE
      };
      optionsNative.NotificationMappings = initialNotifications.ToArray();
      optionsNative.NotificationMappingsCount = optionsNative.NotificationMappings.Length;
      return optionsNative;
    }
  }
}
