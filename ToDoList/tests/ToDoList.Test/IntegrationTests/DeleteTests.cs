namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;

public class DeleteTests : TestsBase
{
    [Fact]
    public async Task DeleteDeleteById_ExistingId_DeletesOnlyTheOneItem_Async()
    {
        // Arrange
        var itemToDelete = new ToDoItem
        {
            Name = "Task to be deleted",
            Description = "This task will be deleted",
            IsCompleted = false,
            Category = "Category of task that will be deleted"
        };
        var itemNotToDelete = new ToDoItem
        {
            Name = "Task not to be deleted",
            Description = "This task will not be deleted",
            IsCompleted = true,
            Category = "Category of task that will not be deleted"

        };
        await DbContext.ToDoItems.AddRangeAsync(itemToDelete, itemNotToDelete);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await Controller.DeleteByIdAsync(itemToDelete.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(DbContext.ToDoItems.Find(itemToDelete.ToDoItemId));
        Assert.NotNull(DbContext.ToDoItems.Find(itemNotToDelete.ToDoItemId));
    }

    [Fact]
    public async Task Delete_NonExistentId_Returns404NotFound_Async()
    {
        // Arrange
        var itemToDelete = new ToDoItem
        {
            Name = "Task not to be deleted",
            Description = "This task will not be deleted",
            IsCompleted = true
        };
        await DbContext.ToDoItems.AddAsync(itemToDelete);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await Controller.DeleteByIdAsync(9999); // 9999 = nonexistent ID

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
