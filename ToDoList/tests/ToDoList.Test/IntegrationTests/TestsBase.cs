namespace ToDoList.Test.IntegrationTests;

using Microsoft.EntityFrameworkCore;
using ToDoList.Persistence;
using ToDoList.WebApi;

public class TestsBase : IDisposable
{
    protected ToDoItemsContext DbContext { get; }
    protected ToDoItemsController Controller { get; }
    public static int ArbitraryNonExistentId => new Random().Next(100000, 1000000); // nevymyslela jsem zatim nic lepsiho. Resp. dalo by se koukat na maxId a od toho se nejak odrazit, ale tim jak testy bezi paralelne, tak to bude vic flaky nez toto reseni.

    public TestsBase()
    {
        DbContext = new("Data Source=../../../data/localdb_test.db");
        Controller = new ToDoItemsController(DbContext);
    }

    public void Dispose()
    {
        try
        {
            DbContext.ToDoItems.RemoveRange(DbContext.ToDoItems);
            _ = DbContext.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            // ignore – some rows may already be gone due to tests running in parallel
        }
    }
}
