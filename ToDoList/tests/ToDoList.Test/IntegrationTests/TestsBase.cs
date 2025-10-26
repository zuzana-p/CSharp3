namespace ToDoList.Test.IntegrationTests;

using ToDoList.Persistence;
using ToDoList.WebApi;

public class TestsBase : IDisposable
{
    protected ToDoItemsContext DbContext { get; }
    protected ToDoItemsController Controller { get; }

    public TestsBase()
    {
        DbContext = new("Data Source=../../../data/localdb_test.db");
        Controller = new ToDoItemsController(DbContext);
    }


    public void Dispose()
    {
        var allEntities = DbContext.ToDoItems.ToList();
        DbContext.RemoveRange(allEntities);
        _ = DbContext.SaveChanges();
        DbContext.Dispose();
    }

}
