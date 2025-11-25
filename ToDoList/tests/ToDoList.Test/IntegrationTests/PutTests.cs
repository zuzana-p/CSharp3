namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PutTests : TestsBase
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task UpdateById_ExistingId_UpdatesAndReturnsNoContent_Async(bool updatedIsCompleted)
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Task to be updated 1",
            Description = "Description to be updated 1",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            Name = "Task to be updated 2",
            Description = "Description to be updated 2",
            IsCompleted = true
        };
        await DbContext.ToDoItems.AddRangeAsync(toDoItem1, toDoItem2);
        await DbContext.SaveChangesAsync();

        string updatedName = "Name after update";
        string updatedDescription = "Description after update";
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto(updatedName, updatedDescription, updatedIsCompleted);

        // Act
        var result = await Controller.UpdateByIdAsync(toDoItem1.ToDoItemId, toDoItemUpdateRequestDto);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var itemsIds = DbContext.ToDoItems.Select(x => x.ToDoItemId);
        Assert.Contains(toDoItem1.ToDoItemId, itemsIds);
        Assert.Contains(toDoItem2.ToDoItemId, itemsIds);

        var updatedItem = await DbContext.ToDoItems.FindAsync(toDoItem1.ToDoItemId);
        Assert.NotNull(updatedItem);
        Assert.Equal(updatedName, updatedItem.Name);
        Assert.Equal(updatedDescription, updatedItem.Description);
        Assert.Equal(updatedIsCompleted, updatedItem.IsCompleted);

        var notUpdatedItem = await DbContext.ToDoItems.FindAsync(toDoItem2.ToDoItemId);
        Assert.NotNull(notUpdatedItem);
        Assert.Equal(toDoItem2.Name, notUpdatedItem.Name);
        Assert.Equal(toDoItem2.Description, notUpdatedItem.Description);
        Assert.Equal(toDoItem2.IsCompleted, notUpdatedItem.IsCompleted);
    }

    [Fact]
    public async Task Put_UpdateByNonExistentId_Returns404NotFound_Async()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "Name not to be updated",
            Description = "Description not to be updated",
            IsCompleted = false
        };
        await DbContext.ToDoItems.AddAsync(toDoItem);
        await DbContext.SaveChangesAsync();
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto("Name after update", "Description after update", true);

        // Act
        var result = await Controller.UpdateByIdAsync(9999, toDoItemUpdateRequestDto); // 9999 = nonexistent ID

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
