namespace ToDoList.Test.IntegrationTests;

using Microsoft.EntityFrameworkCore;
using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class TestsBase : IDisposable
{
    protected ToDoItemsContext DbContext { get; }
    protected ToDoItemsController Controller { get; }
    protected IRepository<ToDoItem> RepositoryMock;

    public TestsBase()
    {
        DbContext = new("Data Source=../../../data/localdb_test.db");
        RepositoryMock = Substitute.For<IRepository<ToDoItem>>();
        Controller = new ToDoItemsController(null, RepositoryMock);
    }

    public void Dispose()
    {
        try
        {
            DbContext.ToDoItems.RemoveRange(DbContext.ToDoItems);
            DbContext.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            // ignore – some rows may already be gone due to tests running in parallel
        }
    }
}
