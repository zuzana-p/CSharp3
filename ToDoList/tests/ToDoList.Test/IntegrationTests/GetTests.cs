namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class GetTests : TestsBase
{
    // Get/Read tests have itemId = 2x
    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 21,
            Name = "Some name 1",
            Description = "Some description 1",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 22,
            Name = "Some name 2",
            Description = "Some description 2",
            IsCompleted = true
        };
        Controller.AddItemToStorage(todoItem1);
        Controller.AddItemToStorage(todoItem2);

        // Act
        object? result = Controller.Read();

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var dtoResult = Assert.IsType<List<ToDoItemGetResponseDto>>(okObjectResult.GetValue());

        var readToDoItem1 = dtoResult.Find(x => x.ToDoItemId == todoItem1.ToDoItemId);
        Assert.NotNull(readToDoItem1);
        Assert.Equal(todoItem1.ToDoItemId, readToDoItem1.ToDoItemId);
        Assert.Equal(todoItem1.Name, readToDoItem1.Name);
        Assert.Equal(todoItem1.Description, readToDoItem1.Description);
        Assert.Equal(todoItem1.IsCompleted, readToDoItem1.IsCompleted);

        var readToDoItem2 = dtoResult.Find(x => x.ToDoItemId == todoItem2.ToDoItemId);
        Assert.NotNull(readToDoItem2);
        Assert.Equal(todoItem2.ToDoItemId, readToDoItem2.ToDoItemId);
        Assert.Equal(todoItem2.Name, readToDoItem2.Name);
        Assert.Equal(todoItem2.Description, readToDoItem2.Description);
        Assert.Equal(todoItem2.IsCompleted, readToDoItem2.IsCompleted);
    }

    [Fact]
    public void Get_NullItems_Returns404NotFound_NOTIMPLEMENTED() => throw new NotImplementedException();

    [Fact]
    public void Get_TODO_Returns500InternalServerError_NOTIMPLEMENTED() => throw new NotImplementedException();

    [Fact]
    public void GetById_ItemId_ReturnsItem()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 21,
            Name = "Some name 1",
            Description = "Some description 1",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 22,
            Name = "Some name 2",
            Description = "Some description 2",
            IsCompleted = true
        };
        Controller.AddItemToStorage(todoItem1);
        Controller.AddItemToStorage(todoItem2);

        // Act
        object? result = Controller.ReadById(22);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var dtoResult = Assert.IsType<ToDoItemGetResponseDto>(okObjectResult.GetValue());

        Assert.Equal(todoItem2.ToDoItemId, dtoResult.ToDoItemId);
        Assert.Equal(todoItem2.Name, dtoResult.Name);
        Assert.Equal(todoItem2.Description, dtoResult.Description);
        Assert.Equal(todoItem2.IsCompleted, dtoResult.IsCompleted);
    }

    [Fact]
    public void GetById_NonExistenstId_Returns404NotFound()
    {
        // Arrange
        var todoItem = new ToDoItem
        {
            ToDoItemId = 21,
            Name = "Some name",
            Description = "Some description",
            IsCompleted = false
        };
        Controller.AddItemToStorage(todoItem);

        // Act
        var result = Controller.ReadById(22);

        // Assert
        _ = Assert.IsType<NotFoundResult>(result);
    }

}
