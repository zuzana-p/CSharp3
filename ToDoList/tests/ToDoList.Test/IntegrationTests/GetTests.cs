namespace ToDoList.Test.IntegrationTests;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Test.TestUtilities;

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
            Category = "Category 1",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            Name = "Name of task 2",
            Description = "Description 2",
            Category = "Category 2",
            IsCompleted = true
        };
        await DbContext.ToDoItems.AddRangeAsync(toDoItem1, toDoItem2);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await Controller.ReadAsync();

        // Assert
        var dtoResult = Assert.IsType<List<ToDoItemGetResponseDto>>(result.GetValue());

        var returnedToDoItem1 = dtoResult.Find(x => x.ToDoItemId == toDoItem1.ToDoItemId);
        returnedToDoItem1.Should().NotBeNull();
        returnedToDoItem1.Should().BeEquivalentTo(toDoItem1);

        var returnedToDoItem2 = dtoResult.Find(x => x.ToDoItemId == toDoItem2.ToDoItemId);
        returnedToDoItem2.Should().NotBeNull();
        returnedToDoItem2.Should().BeEquivalentTo(toDoItem2);
    }

    [Fact]
    public async Task GetById_ItemId_ReturnsItem_Async()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Name of task 1",
            Description = "Description 1",
            Category = "Category 1",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            Name = "Name of task 2",
            Description = "Description 2",
            Category = "Category 2",
            IsCompleted = true
        };
        await DbContext.ToDoItems.AddRangeAsync(toDoItem1, toDoItem2);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await Controller.ReadByIdAsync(toDoItem1.ToDoItemId);

        // Assert
        var dtoResult = result.GetValue();
        dtoResult.Should().BeOfType<ToDoItemGetResponseDto>();
        dtoResult.Should().BeEquivalentTo(toDoItem1);
    }

    [Fact]
    public async Task GetById_NonExistenstId_Returns404NotFound_Async()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Name of task 1",
            Description = "Description 1",
            Category = "Category 1",
            IsCompleted = false
        };
        await DbContext.ToDoItems.AddAsync(toDoItem1);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await Controller.ReadByIdAsync(9999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }
}
