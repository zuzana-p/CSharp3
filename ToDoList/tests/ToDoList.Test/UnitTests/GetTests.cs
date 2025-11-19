namespace ToDoList.Test.UnitTests;

using System.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Test.IntegrationTests;

public class GetTests : TestsBase
{
    [Fact]
    public void Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Name of task 1",
            Description = "Description 1",
            IsCompleted = false
        };
        RepositoryMock.Read().Returns([toDoItem1]);

        // Act
        var result = Controller.Read();

        // Assert
        RepositoryMock.Received(1).Read();

        var dtoResult = Assert.IsType<List<ToDoItemGetResponseDto>>(result.GetValue());

        var returnedToDoItem1 = dtoResult.Find(x => x.ToDoItemId == toDoItem1.ToDoItemId);
        returnedToDoItem1.Should().NotBeNull();
        returnedToDoItem1.Should().BeEquivalentTo(toDoItem1);
    }

    [Fact]
    public void Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        // Arrange

        //Act
        var result = Controller.Read();

        // Assert
        RepositoryMock.Received(1).Read();

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        RepositoryMock.When(x => x.Read()).Do(x => throw new InvalidOperationException());

        // Act
        var result = Controller.Read();

        // Assert
        RepositoryMock.Received(1).Read();

        var objectResult = result.Result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(500);
    }

    [Fact]
    public void Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
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
        RepositoryMock.Received(1).ReadById(1);

        var dtoResult = Assert.IsType<ToDoItemGetResponseDto>(result.GetValue());
        dtoResult.Should().BeEquivalentTo(toDoItem1);
    }

    [Fact]
    public void Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.ReadById(999).Returns(null as ToDoItem);

        // Act
        var result = Controller.ReadById(999);

        // Assert
        RepositoryMock.Received(1).ReadById(999);

        result.Result.Should().BeOfType<NotFoundResult>();

    }

    [Fact]
    public void Get_ReadByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        RepositoryMock.When(x => x.ReadById(1)).Do(x => throw new InvalidOperationException());

        // Act
        var result = Controller.ReadById(1);

        // Assert
        RepositoryMock.Received(1).ReadById(1);

        var objectResult = result.Result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(500);
    }
}
