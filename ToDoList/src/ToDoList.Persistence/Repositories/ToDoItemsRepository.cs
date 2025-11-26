namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Exceptions;
using ToDoList.Domain.Models;


public class ToDoItemsRepository(ToDoItemsContext dbContext) : IRepositoryAsync<ToDoItem>
{
    private readonly ToDoItemsContext dbContext = dbContext;

    public async Task CreateAsync(ToDoItem item)
    {
        await dbContext.AddAsync(item);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ToDoItem>> ReadAsync() => await dbContext.ToDoItems.ToListAsync();

    public async Task<ToDoItem?> ReadByIdAsync(int id) => await dbContext.ToDoItems.FindAsync(id);

    public async Task UpdateByIdAsync(ToDoItem toDoItemValuesAfterUpdate)
    {
        var toDoItem = await dbContext.ToDoItems.FindAsync(toDoItemValuesAfterUpdate.ToDoItemId);

        if (toDoItem != null)
        {
            dbContext.Entry(toDoItem).CurrentValues.SetValues(toDoItemValuesAfterUpdate);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new EntityNotFoundException(nameof(ToDoItem), toDoItemValuesAfterUpdate.ToDoItemId);
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
