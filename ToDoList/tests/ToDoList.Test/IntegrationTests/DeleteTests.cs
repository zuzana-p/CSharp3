namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;

public class DeleteTests : TestsBase
{
    [Fact]
    public void DeleteDeleteById_ExistingId_DeletesOnlyTheOneItem()
    {
        // Arrange
        var itemToDelete = new ToDoItem
        {
            Name = "Task to be deleted",
            Description = "This task will be deleted",
            IsCompleted = false
        };
        var itemNotToDelete = new ToDoItem
        {
            Name = "Task not to be deleted",
            Description = "This task will not be deleted",
            IsCompleted = true
        };
        DbContext.ToDoItems.AddRange(itemToDelete, itemNotToDelete);
        DbContext.SaveChanges();

        // Act
        var result = Controller.DeleteById(itemToDelete.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(DbContext.ToDoItems.Find(itemToDelete.ToDoItemId));
        Assert.NotNull(DbContext.ToDoItems.Find(itemNotToDelete.ToDoItemId));
    }

    [Fact]
    public void Delete_NonExistentId_Returns404NotFound()
    {
        // Arrange
        var itemToDelete = new ToDoItem
        {
            Name = "Task not to be deleted",
            Description = "This task will not be deleted",
            IsCompleted = true
        };
        DbContext.ToDoItems.Add(itemToDelete);
        DbContext.SaveChanges();

        // Act
        var result = Controller.DeleteById(9999); // 9999 = nonexistent ID

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
