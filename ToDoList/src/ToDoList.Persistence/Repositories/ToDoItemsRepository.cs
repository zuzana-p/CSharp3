namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Exceptions;
using ToDoList.Domain.Models;


public class ToDoItemsRepository : IRepositoryAsync<ToDoItem>
{
    private readonly ToDoItemsContext dbContext;

    public ToDoItemsRepository(ToDoItemsContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CreateAsync(ToDoItem item)
    {
        await dbContext.AddAsync(item);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ToDoItem>> ReadAsync() => await dbContext.ToDoItems.ToListAsync();

    public async Task<ToDoItem?> ReadByIdAsync(int id) => await dbContext.ToDoItems.FindAsync(id);

    public async Task UpdateByIdAsync(int id, ToDoItem toDoItemValuesAfterUpdate)
    {
        var toDoItem = await dbContext.ToDoItems.FindAsync(id);

        if (toDoItem != null)
        {
            toDoItem.Name = toDoItemValuesAfterUpdate.Name; // noteZPA Context.Entry(foundItem).CurrentValues.SetValues(item)
            toDoItem.Description = toDoItemValuesAfterUpdate.Description;
            toDoItem.IsCompleted = toDoItemValuesAfterUpdate.IsCompleted;

            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new EntityNotFoundException(nameof(ToDoItem), id);
        }
    }

    public async Task DeleteByIdAsync(int id)
    {
        var toDoItemToDelete = await dbContext.ToDoItems.FindAsync(id);

        if (toDoItemToDelete != null)
        {
            dbContext.ToDoItems.Remove(toDoItemToDelete);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new EntityNotFoundException(nameof(ToDoItem), id);
        }
    }
}
