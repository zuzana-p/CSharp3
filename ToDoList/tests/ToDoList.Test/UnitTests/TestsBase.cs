namespace ToDoList.Test.UnitTests;

using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class TestsBase
{
    protected ToDoItemsController Controller { get; }
    protected IRepositoryAsync<ToDoItem> RepositoryMock { get; }

    public TestsBase()
    {
        RepositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        Controller = new ToDoItemsController(RepositoryMock);
    }
}
