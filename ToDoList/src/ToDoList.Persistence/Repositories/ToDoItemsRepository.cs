namespace ToDoList.Persistence.Repositories;

using System.Data;
using ToDoList.Domain.Exceptions;
using ToDoList.Domain.Models;


public class ToDoItemsRepository(ToDoItemsContext dbContext) : IRepository<ToDoItem>
{
    private readonly ToDoItemsContext dbContext = dbContext;

    public void Create(ToDoItem item)
    {
        dbContext.Add(item);
        dbContext.SaveChanges();
    }

    public IEnumerable<ToDoItem> Read() => [.. dbContext.ToDoItems];

    public ToDoItem? ReadById(int id) => dbContext.ToDoItems.Find(id);

    public void UpdateById(int id, ToDoItem toDoItemValuesAfterUpdate)
    {
        var toDoItem = dbContext.ToDoItems.Find(id);

        if (toDoItem != null)
        {
            toDoItem.Name = toDoItemValuesAfterUpdate.Name; // noteZPA Context.Entry(foundItem).CurrentValues.SetValues(item)
            toDoItem.Description = toDoItemValuesAfterUpdate.Description;
            toDoItem.IsCompleted = toDoItemValuesAfterUpdate.IsCompleted;

            dbContext.SaveChanges();
        }
        else
        {
            throw new EntityNotFoundException(nameof(ToDoItem), id);
        }
    }

    public void DeleteById(int id)
    {
        var toDoItemToDelete = dbContext.ToDoItems.Find(id);

        if (toDoItemToDelete != null)
        {
            dbContext.ToDoItems.Remove(toDoItemToDelete);
            dbContext.SaveChanges();
        }
        else
        {
            throw new EntityNotFoundException(nameof(ToDoItem), id);
        }
    }
}
