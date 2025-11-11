namespace ToDoList.Test.UnitTests;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PostTests : TestsBase
{
    public PostTests()
    {
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Post_CreateValidRequest_ReturnsCreatedAtAction(bool isCompleted)
    {
        // Arrange
        string itemName = "Name of task";
        string itemDescription = "Description of task";
        var toDoItemCreateRequestDto = new ToDoItemCreateRequestDto(itemName, itemDescription, isCompleted);

        // Act
        var result = Controller.Create(toDoItemCreateRequestDto);

        // Assert
        RepositoryMock.Received(1).Create(Arg.Any<ToDoItem>());

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        createdAtActionResult.ActionName.Should().Be("ReadById");

        var todoItemResponseDto = createdAtActionResult.Value as ToDoItemGetResponseDto;
        todoItemResponseDto.Should().NotBeNull();

        createdAtActionResult.RouteValues.Should().NotBeNull();
        createdAtActionResult.RouteValues["toDoItemId"].Should().Be(todoItemResponseDto.ToDoItemId);

        todoItemResponseDto.Should().BeEquivalentTo(new
        {
            Name = itemName,
            Description = itemDescription,
            IsCompleted = isCompleted
        });
    }

    [Fact]
    public void Post_CreateUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var toDoItemCreateRequestDto = new ToDoItemCreateRequestDto("Name", "Description", false);
        RepositoryMock.When(x => x.Create(Arg.Any<ToDoItem>())).Do(_ => throw new InvalidOperationException());

        // Act
        var result = Controller.Create(toDoItemCreateRequestDto);

        // Assert
        RepositoryMock.Received(1).Create(Arg.Any<ToDoItem>());

        var objectResult = result.Result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(500);
    }

}
