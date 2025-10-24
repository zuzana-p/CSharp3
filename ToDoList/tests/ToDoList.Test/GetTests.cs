namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class GetTests : TestsBase
{
    //[Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Some name 1",
            Description = "Some description 1",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
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

        var firstToDoItem = dtoResult.First();
        Assert.Equal(todoItem1.ToDoItemId, firstToDoItem.ToDoItemId);
        Assert.Equal(todoItem1.Name, firstToDoItem.Name);
        Assert.Equal(todoItem1.Description, firstToDoItem.Description);
        Assert.Equal(todoItem1.IsCompleted, firstToDoItem.IsCompleted);

        var secondToDoItem = dtoResult.Last();
        Assert.Equal(todoItem2.ToDoItemId, secondToDoItem.ToDoItemId);
        Assert.Equal(todoItem2.Name, secondToDoItem.Name);
        Assert.Equal(todoItem2.Description, secondToDoItem.Description);
        Assert.Equal(todoItem2.IsCompleted, secondToDoItem.IsCompleted);
    }

    [Fact]
    public void Get_NullItems_Returns404NotFound()
    {
        // Arrange

        // Act
        var result = Controller.Read();

        // Assert
        _ = Assert.IsType<NotFoundResult>(result);
    }

    //[Fact]
    public void Get_TODO_Returns500InternalServerError() => throw new NotImplementedException();

    //[Fact]
    public void GetById_ItemId_ReturnsItem()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Some name 1",
            Description = "Some description 1",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Some name 2",
            Description = "Some description 2",
            IsCompleted = true
        };
        Controller.AddItemToStorage(todoItem1);
        Controller.AddItemToStorage(todoItem2);

        // Act
        object? result = Controller.ReadById(2);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var dtoResult = Assert.IsType<ToDoItemGetResponseDto>(okObjectResult.GetValue());

        Assert.Equal(todoItem2.ToDoItemId, dtoResult.ToDoItemId);
        Assert.Equal(todoItem2.Name, dtoResult.Name);
        Assert.Equal(todoItem2.Description, dtoResult.Description);
        Assert.Equal(todoItem2.IsCompleted, dtoResult.IsCompleted);
    }

    [Fact]
    public void GetById_NullItem_Returns404NotFound()
    {
        // Arrange
        var todoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Some name",
            Description = "Some description",
            IsCompleted = false
        };
        Controller.AddItemToStorage(todoItem);

        // Act
        var result = Controller.ReadById(2);

        // Assert
        _ = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetById_NullItems_Returns500InternalServerError()
    {
        //Arrange

        //Act
        var objectResult = Controller.ReadById(2) as ObjectResult;

        //Assert
        Assert.Equal(500, objectResult.StatusCode);
    }
}
