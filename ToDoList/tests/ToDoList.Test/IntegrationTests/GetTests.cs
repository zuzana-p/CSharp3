namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class GetTests : TestsBase
{
    [Fact]
    public async Task Get_AllItems_ReturnsAllItems_Async()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Name of task 1",
            Description = "Description 1",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            Name = "Name of task 2",
            Description = "Description 2",
            IsCompleted = true
        };
        await DbContext.ToDoItems.AddRangeAsync(toDoItem1, toDoItem2);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await Controller.ReadAsync();

        // Assert
        var dtoResult = Assert.IsType<List<ToDoItemGetResponseDto>>(result.GetValue());

        var returnedToDoItem1 = dtoResult.Find(x => x.ToDoItemId == toDoItem1.ToDoItemId);
        Assert.NotNull(returnedToDoItem1);
        Assert.Equal(toDoItem1.ToDoItemId, returnedToDoItem1.ToDoItemId);
        Assert.Equal(toDoItem1.Name, returnedToDoItem1.Name);
        Assert.Equal(toDoItem1.Description, returnedToDoItem1.Description);
        Assert.Equal(toDoItem1.IsCompleted, returnedToDoItem1.IsCompleted);

        var returnedToDoItem2 = dtoResult.Find(x => x.ToDoItemId == toDoItem2.ToDoItemId);
        Assert.NotNull(returnedToDoItem2);
        Assert.Equal(toDoItem2.ToDoItemId, returnedToDoItem2.ToDoItemId);
        Assert.Equal(toDoItem2.Name, returnedToDoItem2.Name);
        Assert.Equal(toDoItem2.Description, returnedToDoItem2.Description);
        Assert.Equal(toDoItem2.IsCompleted, returnedToDoItem2.IsCompleted);
    }

    [Fact]
    public async Task GetById_ItemId_ReturnsItem_Async()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Name of task 1",
            Description = "Description 1",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            Name = "Name of task 2",
            Description = "Description 2",
            IsCompleted = true
        };
        await DbContext.ToDoItems.AddRangeAsync(toDoItem1, toDoItem2);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await Controller.ReadByIdAsync(toDoItem1.ToDoItemId);

        // Assert
        var dtoResult = Assert.IsType<ToDoItemGetResponseDto>(result.GetValue());
        Assert.Equal(toDoItem1.ToDoItemId, dtoResult.ToDoItemId);
        Assert.Equal(toDoItem1.Name, dtoResult.Name);
        Assert.Equal(toDoItem1.Description, dtoResult.Description);
        Assert.Equal(toDoItem1.IsCompleted, dtoResult.IsCompleted);
    }

    [Fact]
    public async Task GetById_NonExistenstId_Returns404NotFound_Async()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Name of task 1",
            Description = "Description 1",
            IsCompleted = false
        };
        await DbContext.ToDoItems.AddAsync(toDoItem1);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await Controller.ReadByIdAsync(9999); // 9999 = nonexistent ID

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}
