namespace ToDoList.Test;

using ToDoList.Persistence;
using ToDoList.WebApi;

public class TestsBase : IDisposable
{
    protected ToDoItemsContext DbContext { get; }
    protected ToDoItemsController Controller { get; }

    public TestsBase(int maxUsedId)
    {
        //DbContext = new("DataSource=../data/unitTestsDb.db"); // Q: jaká je praxe? Spíš k testům nebo do filu data?
        DbContext = new("DataSource=unitTestsDb.db", maxUsedId);
        Controller = new ToDoItemsController(DbContext);
    }


    public void Dispose()
    {
        var allEntities = DbContext.ToDoItems.ToList();
        DbContext.RemoveRange(allEntities);
        DbContext.SaveChanges();
        DbContext.Dispose();
    }

}
