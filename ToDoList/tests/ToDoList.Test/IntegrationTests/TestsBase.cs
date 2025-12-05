namespace ToDoList.Test.IntegrationTests;

using Microsoft.EntityFrameworkCore;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class TestsBase : IDisposable
{
    protected ToDoItemsContext DbContext { get; }
    protected ToDoItemsController Controller { get; }

    public TestsBase()
    {
        DbContext = new("Data Source=../../../data/localdb_test.db");
        DbContext.Database.Migrate();
        var repository = new ToDoItemsRepository(DbContext);
        Controller = new ToDoItemsController(repository);
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
        GC.SuppressFinalize(this);
    }
}
