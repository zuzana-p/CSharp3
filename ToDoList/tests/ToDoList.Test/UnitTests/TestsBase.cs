namespace ToDoList.Test.UnitTests;

using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class TestsBase
{
    protected ToDoItemsController Controller { get; }
    protected IRepository<ToDoItem> RepositoryMock;

    public TestsBase()
    {
        RepositoryMock = Substitute.For<IRepository<ToDoItem>>();
        Controller = new ToDoItemsController(RepositoryMock);
    }
}
