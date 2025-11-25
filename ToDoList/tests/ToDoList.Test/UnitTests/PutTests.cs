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
    public async Task Put_UpdateByIdWhenItemUpdated_ReturnsNoContent_Async(bool updatedIsCompleted)
    {
        // Arrange
        string updatedName = "Name after update";
        string updatedDescription = "Description after update";
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto(updatedName, updatedDescription, updatedIsCompleted);
        await RepositoryMock.UpdateByIdAsync(1, Arg.Any<ToDoItem>());

        // Act
        var result = await Controller.UpdateByIdAsync(1, toDoItemUpdateRequestDto);

        // Assert
        await RepositoryMock.Received(1).UpdateByIdAsync(1, Arg.Any<ToDoItem>());

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Put_UpdateByIdWhenIdNotFound_ReturnsNotFound_Async()
    {
        // Arrange
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto("Name after update", "Description after update", true);
        RepositoryMock
            .When(x => x.UpdateByIdAsync(999, Arg.Any<ToDoItem>()))
            .Do(x => throw new EntityNotFoundException(nameof(ToDoItem), 999));

        // Act
        var result = await Controller.UpdateByIdAsync(999, toDoItemUpdateRequestDto);

        // Assert
        await RepositoryMock.Received(1).UpdateByIdAsync(999, Arg.Any<ToDoItem>());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Put_UpdateByIdUnhandledException_ReturnsInternalServerError_Async()
    {
        // Arrange
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto("Name after update", "Description after update", true);
        RepositoryMock
                    .When(x => x.UpdateByIdAsync(1, Arg.Any<ToDoItem>()))
                    .Do(x => throw new InvalidOperationException());

        // Act
        var result = await Controller.UpdateByIdAsync(1, toDoItemUpdateRequestDto);

        // Assert
        await RepositoryMock.Received(1).UpdateByIdAsync(1, Arg.Any<ToDoItem>());

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(500);
    }
}
