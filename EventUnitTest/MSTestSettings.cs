using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Services;

[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]

[TestClass]

public sealed class EventTest 
{
    public IEventServiceAsync IESA=new EventServicesAsync();
}