namespace ToDoList.Test.UnitTests;

using System.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Test.TestUtilities;

public class GetTests : TestsBase
{
    [Fact]
    public async Task Get_ReadWhenSomeItemAvailable_ReturnsOk_Async()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Name of task 1",
            Description = "Description 1",
            Category = "Category 1",
            IsCompleted = false
        };
        RepositoryMock.ReadAsync().Returns([toDoItem1]);

        // Act
        var result = await Controller.ReadAsync();

        // Assert
        await RepositoryMock.Received(1).ReadAsync();

        var dtoResult = Assert.IsType<List<ToDoItemGetResponseDto>>(result.GetValue());

        var returnedToDoItem1 = dtoResult.Find(x => x.ToDoItemId == toDoItem1.ToDoItemId);
        returnedToDoItem1.Should().NotBeNull();
        returnedToDoItem1.Should().BeEquivalentTo(toDoItem1);
    }

    [Fact]
    public async Task Get_ReadWhenNoItemAvailable_ReturnsNotFound_Async()
    {
        // Arrange

        //Act
        var result = await Controller.ReadAsync();

        // Assert
        await RepositoryMock.Received(1).ReadAsync();

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Get_ReadUnhandledException_ReturnsInternalServerError_Async()
    {
        // Arrange
        RepositoryMock.When(x => x.ReadAsync()).Do(x => throw new InvalidOperationException());

        // Act
        var result = await Controller.ReadAsync();

        // Assert
        await RepositoryMock.Received(1).ReadAsync();

        var objectResult = result.Result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task Get_ReadByIdWhenSomeItemAvailable_ReturnsOk_Async()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Name of task 1",
            Description = "Description 1",
            Category = "Category 1",
            IsCompleted = false
        };
        RepositoryMock.ReadByIdAsync(1).Returns(toDoItem1);

        // Act
        var result = await Controller.ReadByIdAsync(toDoItem1.ToDoItemId);

        // Assert
        await RepositoryMock.Received(1).ReadByIdAsync(1);

        var dtoResult = Assert.IsType<ToDoItemGetResponseDto>(result.GetValue());
        dtoResult.Should().BeEquivalentTo(toDoItem1);
    }

    [Fact]
    public async Task Get_ReadByIdWhenItemIsNull_ReturnsNotFound_Async()
    {
        // Arrange
        RepositoryMock.ReadByIdAsync(999).Returns(null as ToDoItem);

        // Act
        var result = await Controller.ReadByIdAsync(999);

        // Assert
        await RepositoryMock.Received(1).ReadByIdAsync(999);

        result.Result.Should().BeOfType<NotFoundResult>();

    }

    [Fact]
    public async Task Get_ReadByIdUnhandledException_ReturnsInternalServerError_Async()
    {
        // Arrange
        RepositoryMock.When(x => x.ReadByIdAsync(1)).Do(x => throw new InvalidOperationException());

        // Act
        var result = await Controller.ReadByIdAsync(1);

        // Assert
        await RepositoryMock.Received(1).ReadByIdAsync(1);

        var objectResult = result.Result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(500);
    }
}
