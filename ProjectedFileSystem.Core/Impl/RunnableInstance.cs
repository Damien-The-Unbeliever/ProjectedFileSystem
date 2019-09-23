using ProjectedFileSystem.Core.Interfaces;
using ProjectedFileSystem.Core.Native;
using System;

namespace ProjectedFileSystem.Core.Impl
{
  internal class RunnableInstance : IRunnableInstance
  {
    private readonly Functions _outboundFunctions;
    public RunnableInstance(string name, string rootPath, Guid instanceGuid, InstanceOptions options, Functions outboundFunctions)
    {
      Name = name;
      RootPath = rootPath;
      InstanceGuid = instanceGuid;
      DefaultOptions = options;
      _outboundFunctions = outboundFunctions;
    }
    public string Name { get; }

    public Guid InstanceGuid { get; }

    public string RootPath { get; }
    public InstanceOptions DefaultOptions { get; }

    public IRunningInstance Start(IFileSystem fileSystem, InstanceOptions overrideOptions)
    {
      return new RunningInstance(fileSystem, overrideOptions, this, _outboundFunctions);
    }

    public IRunningInstance Start(IFileSystem fileSystem)
    {
      return Start(fileSystem, DefaultOptions);
    }
  }
}
