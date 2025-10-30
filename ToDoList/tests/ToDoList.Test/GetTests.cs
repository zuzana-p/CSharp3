namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class GetTests
{
    [Fact]
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
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(todoItem1);
        controller.AddItemToStorage(todoItem2);

        // Act
        object? result = controller.Read();

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
        var controller = new ToDoItemsController(null as List<ToDoItem>);

        // Act
        var result = controller.Read();

        // Assert
        _ = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Get_TODO_Returns500InternalServerError()
    {
        // TODO: nevím jak simulovat exception
    }

    [Fact]
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
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(todoItem1);
        controller.AddItemToStorage(todoItem2);

        // Act
        object? result = controller.ReadById(2);

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
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(todoItem);

        // Act
        var result = controller.ReadById(2);

        // Assert
        _ = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetById_NullItems_Returns500InternalServerError()
    {
        //Arrange
        var controller = new ToDoItemsController(null as List<ToDoItem>);

        //Act
        var objectResult = controller.ReadById(2) as ObjectResult;

        //Assert
        Assert.Equal(500, objectResult.StatusCode);
    }
}
