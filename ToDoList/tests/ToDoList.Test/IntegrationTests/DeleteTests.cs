namespace ToDoList.Test.IntegrationTests;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;

public class DeleteTests : TestsBase
{
    [Fact]
    public async Task DeleteById_ExistingId_DeletesOnlyTheOneItem_Async()
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
        result.Should().BeOfType<NoContentResult>();
        DbContext.ToDoItems.Find(itemToDelete.ToDoItemId).Should().BeNull();
        DbContext.ToDoItems.Find(itemNotToDelete.ToDoItemId).Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_NonExistentId_Returns404NotFound_Async()
    {
        // Arrange
        var itemToDelete = new ToDoItem
        {
            Name = "Task not to be deleted",
            Description = "This task will not be deleted",
            Category = "Category not to be deleted",
            IsCompleted = true
        };
        await DbContext.ToDoItems.AddAsync(itemToDelete);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await Controller.DeleteByIdAsync(9999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
