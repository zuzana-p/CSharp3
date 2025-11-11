namespace ToDoList.Test.UnitTests;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Exceptions;
using ToDoList.Domain.Models;

public class PutTests : TestsBase
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Put_UpdateByIdWhenItemUpdated_ReturnsNoContent(bool updatedIsCompleted)
    {
        // Arrange
        string updatedName = "Name after update";
        string updatedDescription = "Description after update";
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto(updatedName, updatedDescription, updatedIsCompleted);
        RepositoryMock.UpdateById(1, Arg.Any<ToDoItem>());

        // Act
        var result = Controller.UpdateById(1, toDoItemUpdateRequestDto);

        // Assert
        RepositoryMock.Received(1).UpdateById(1, Arg.Any<ToDoItem>());

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        // Arrange
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto("Name after update", "Description after update", true);
        RepositoryMock
            .When(x => x.UpdateById(999, Arg.Any<ToDoItem>()))
            .Do(x => throw new EntityNotFoundException(nameof(ToDoItem), 999));

        // Act
        var result = Controller.UpdateById(999, toDoItemUpdateRequestDto);

        // Assert
        RepositoryMock.Received(1).UpdateById(999, Arg.Any<ToDoItem>());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto("Name after update", "Description after update", true);
        RepositoryMock
                    .When(x => x.UpdateById(1, Arg.Any<ToDoItem>()))
                    .Do(x => throw new InvalidOperationException());

        // Act
        var result = Controller.UpdateById(1, toDoItemUpdateRequestDto);

        // Assert
        RepositoryMock.Received(1).UpdateById(1, Arg.Any<ToDoItem>());

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(500);
    }
}
