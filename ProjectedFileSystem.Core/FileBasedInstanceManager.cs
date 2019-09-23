using ProjectedFileSystem.Core.FileSystem;
using ProjectedFileSystem.Core.Impl;
using ProjectedFileSystem.Core.Interfaces;
using ProjectedFileSystem.Core.Native;
using System;
using System.Buffers.Binary;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core
{
  /// <summary>
  /// An instance manager that stores its instance details in a file
  /// </summary>
  public sealed class FileBasedInstanceManager : IInstanceManager, IDisposable
  {
    private const string UnexpectedFF = "Unexpected file format";
    private readonly FileStream _dataFile;
    private readonly Dictionary<string, IRunnableInstance> _names;
    private readonly Dictionary<string, IRunnableInstance> _paths;
    private readonly Dictionary<Guid, IRunnableInstance> _guids;
    private readonly object _instanceLock;
    private readonly Functions _outboundFunctions;

    /// <summary>
    /// The name of the file being used to back this instance of the manager
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Initializes the <see cref="IInstanceManager"/> with its backing data file
    /// </summary>
    /// <param name="fileName">The name of the file to use</param>
    /// <remarks>If the file does not exist, it'll be created</remarks>
    internal FileBasedInstanceManager(string fileName, Functions outboundFunctions)
    {
      FileName = fileName;
      _outboundFunctions = outboundFunctions;
      _dataFile = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
      _names = new Dictionary<string, IRunnableInstance>();
      _paths = new Dictionary<string, IRunnableInstance>();
      _guids = new Dictionary<Guid, IRunnableInstance>();
      _instanceLock = new object();
      while (ReadRecord()) { }
    }
    public FileBasedInstanceManager(string fileName) : this(fileName,new Functions.DefaultBinding())
    {

    }

    void IInstanceManager.Deregister(Guid guid)
    {
      lock (_instanceLock)
      {
        if (!_guids.TryGetValue(guid, out var instance)) throw new ArgumentOutOfRangeException(nameof(guid), "Instance not registered");
        Directory.Delete(instance.RootPath, true);
        _guids.Remove(guid);
        _names.Remove(instance.Name);
        _paths.Remove(instance.RootPath);
        _dataFile.Seek(0, SeekOrigin.Begin);
        foreach(var otherInstance in _guids.Values)
        {
          WriteRecord(otherInstance);
        }
        _dataFile.Flush();
      }
    }

    void IDisposable.Dispose()
    {
      _dataFile.Close();
    }

    IRunnableInstance IInstanceManager.FindByGuid(Guid guid)
    {
      lock (_instanceLock)
      {
        if (_guids.TryGetValue(guid, out var instance)) return instance;
        return null;
      }
    }

    IRunnableInstance IInstanceManager.FindByName(string name)
    {
      lock (_instanceLock)
      {
        if (_names.TryGetValue(name, out var instance)) return instance;
        return null;
      }
    }

    IRunnableInstance IInstanceManager.FindByRootPath(string rootPath)
    {
      lock (_instanceLock)
      {
        if (_paths.TryGetValue(rootPath, out var instance)) return instance;
        return null;
      }
    }

    IRunnableInstance IInstanceManager.Register(string name, string rootPath, string targetPath, InstanceOptions defaultOptions, PlaceholderVersion rootVersionInfo, Guid instanceGuid)
    {
      lock (_instanceLock)
      {
        if (_names.ContainsKey(name)) throw new ArgumentException("Name already registered", nameof(name));
        if (_paths.ContainsKey(rootPath)) throw new ArgumentException("Path already registered", nameof(rootPath));
        if (_guids.ContainsKey(instanceGuid)) throw new ArgumentException("Instance already registered", nameof(instanceGuid));
        if (_outboundFunctions.PrjMarkDirectoryAsPlaceholder(rootPath, targetPath, LevelShifter.PRJ_PLACEHOLDER_VERSION_INFOFromPlaceholderVersion(rootVersionInfo), instanceGuid) != HRESULT.S_OK) throw new NotSupportedException("Could not register path with System");
        var instance = new RunnableInstance(name, rootPath, instanceGuid, defaultOptions, _outboundFunctions);
        _names.Add(name, instance);
        _paths.Add(rootPath, instance);
        _guids.Add(instanceGuid, instance);
        WriteRecord(instance);
        _dataFile.Flush();
        return instance;
      }
    }

    private bool ReadRecord()
    {
      byte[] smallBuffer = new byte[36];
      var bytesRead = _dataFile.Read(smallBuffer, 0, smallBuffer.Length);
      if(bytesRead==0) return false;
      if (bytesRead != smallBuffer.Length) throw new NotSupportedException(UnexpectedFF);
      if (smallBuffer[0] != 0xFF) throw new NotSupportedException(UnexpectedFF);
      var options = new InstanceOptions();
      if ((smallBuffer[1] & 1) == 1) options.NegativePathCache = true;
      options.PoolThreadCount = BinaryPrimitives.ReadInt32BigEndian(smallBuffer.AsSpan(4, 4));
      options.ConcurrentThreadCount = BinaryPrimitives.ReadInt32BigEndian(smallBuffer.AsSpan(8, 4));
      var instanceGuid = new Guid(smallBuffer.AsSpan(12, 16).ToArray());
      var nameLength = BinaryPrimitives.ReadInt32BigEndian(smallBuffer.AsSpan(28, 4));
      var pathLength = BinaryPrimitives.ReadInt32BigEndian(smallBuffer.AsSpan(32, 4));
      var nameBuffer = new byte[nameLength];
      var pathBuffer = new byte[pathLength];
      if (_dataFile.Read(nameBuffer, 0, nameLength) != nameLength) throw new NotSupportedException(UnexpectedFF);
      if (_dataFile.Read(pathBuffer, 0, pathLength) != pathLength) throw new NotSupportedException(UnexpectedFF);
      var name = Encoding.UTF8.GetString(nameBuffer);
      var path = Encoding.UTF8.GetString(pathBuffer);
      if (_names.ContainsKey(name) || _paths.ContainsKey(path) || _guids.ContainsKey(instanceGuid)) throw new NotSupportedException(UnexpectedFF);
      var runnable = new RunnableInstance(name, path, instanceGuid, options, _outboundFunctions);
      _names.Add(name, runnable);
      _paths.Add(path, runnable);
      _guids.Add(instanceGuid, runnable);
      return true;
    }
    private void WriteRecord(IRunnableInstance instance)
    {
      byte[] smallBuffer = new byte[36];
      smallBuffer[0] = 0xFF;
      if (instance.DefaultOptions.NegativePathCache) smallBuffer[1] = 1;
      BinaryPrimitives.WriteInt32BigEndian(smallBuffer.AsSpan(4, 4), instance.DefaultOptions.PoolThreadCount);
      BinaryPrimitives.WriteInt32BigEndian(smallBuffer.AsSpan(8, 4), instance.DefaultOptions.ConcurrentThreadCount);
      instance.InstanceGuid.ToByteArray().CopyTo(smallBuffer, 12);
      var nameBuffer = Encoding.UTF8.GetBytes(instance.Name);
      var pathBuffer = Encoding.UTF8.GetBytes(instance.RootPath);
      BinaryPrimitives.WriteInt32BigEndian(smallBuffer.AsSpan(28, 4), nameBuffer.Length);
      BinaryPrimitives.WriteInt32BigEndian(smallBuffer.AsSpan(32, 4), pathBuffer.Length);
      _dataFile.Write(smallBuffer, 0, smallBuffer.Length);
      _dataFile.Write(nameBuffer, 0, nameBuffer.Length);
      _dataFile.Write(pathBuffer, 0, pathBuffer.Length);
    }
  }
}
