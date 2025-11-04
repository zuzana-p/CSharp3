namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PutTests : TestsBase
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void UpdateById_ExistingId_UpdatesAndReturnsNoContent(bool updatedIsCompleted)
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
        DbContext.ToDoItems.AddRange(toDoItem1, toDoItem2);
        DbContext.SaveChanges();
        string updatedName = "Name after update";
        string updatedDescription = "Description after update";
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto(updatedName, updatedDescription, updatedIsCompleted);

        // Act
        var result = Controller.UpdateById(toDoItem1.ToDoItemId, toDoItemUpdateRequestDto);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var itemsIds = DbContext.ToDoItems.Select(x => x.ToDoItemId);
        Assert.Equal(2, itemsIds.Count());
        Assert.Contains(toDoItem1.ToDoItemId, itemsIds);
        Assert.Contains(toDoItem2.ToDoItemId, itemsIds);

        var updatedItem = DbContext.ToDoItems.Find(toDoItem1.ToDoItemId);
        Assert.NotNull(updatedItem);
        Assert.Equal(updatedName, updatedItem.Name);
        Assert.Equal(updatedDescription, updatedItem.Description);
        Assert.Equal(updatedIsCompleted, updatedItem.IsCompleted);

        var notUpdatedItem = DbContext.ToDoItems.Find(toDoItem2.ToDoItemId);
        Assert.NotNull(notUpdatedItem);
        Assert.Equal(toDoItem2.Name, notUpdatedItem.Name);
        Assert.Equal(toDoItem2.Description, notUpdatedItem.Description);
        Assert.Equal(toDoItem2.IsCompleted, notUpdatedItem.IsCompleted);
    }

    [Fact]
    public void Put_UpdateByNonExistentId_Returns404NotFound()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "Name not to be updated",
            Description = "Description not to be updated",
            IsCompleted = false
        };
        DbContext.ToDoItems.Add(toDoItem);
        DbContext.SaveChanges();
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto("Name after update", "Description after update", true);

        // Act
        var result = Controller.UpdateById(9999, toDoItemUpdateRequestDto); // 9999 = nonexistent ID

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    // [Fact]
    // public void Put_TODO_Return500InternalServerError_NOTIMPLEMENTED() => throw new NotImplementedException();
}
