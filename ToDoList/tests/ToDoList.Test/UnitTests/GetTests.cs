namespace ToDoList.Test.UnitTests;

using System.Data;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Test.IntegrationTests;

public class GetTests : TestsBase
{
    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Name of task 1",
            Description = "Description 1",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Name of task 2",
            Description = "Description 2",
            IsCompleted = true
        };
        RepositoryMock.Read().Returns([toDoItem1, toDoItem2]);

        // Act
        var result = Controller.Read();

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
    public void Get_NoItems_Returns404NotFound()
    {
        // Arrange

        //Act
        var result = Controller.Read();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Get_RepositoryException_Returns500InternalServerError()
    {
        // Arrange
        RepositoryMock.When(x => x.Read()).Do(x => throw new InvalidOperationException());

        // Act
        var result = Controller.Read();

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public void GetById_ItemId_ReturnsItem()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Name of task 1",
            Description = "Description 1",
            IsCompleted = false
        };
        RepositoryMock.ReadById(1).Returns(toDoItem1);

        // Act
        var result = Controller.ReadById(toDoItem1.ToDoItemId);

        // Assert
        var dtoResult = Assert.IsType<ToDoItemGetResponseDto>(result.GetValue());
        Assert.Equal(toDoItem1.ToDoItemId, dtoResult.ToDoItemId);
        Assert.Equal(toDoItem1.Name, dtoResult.Name);
        Assert.Equal(toDoItem1.Description, dtoResult.Description);
        Assert.Equal(toDoItem1.IsCompleted, dtoResult.IsCompleted);
    }

    [Fact]
    public void GetById_NonExistenstId_Returns404NotFound()
    {
        // Arrange
        RepositoryMock.ReadById(999).Returns(null as ToDoItem);

        // Act
        var result = Controller.ReadById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}
