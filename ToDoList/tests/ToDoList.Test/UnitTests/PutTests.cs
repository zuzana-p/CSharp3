namespace ToDoList.Test.UnitTests;

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
    public void UpdateById_ExistingId_UpdatesAndReturnsNoContent(bool updatedIsCompleted)
    {
        // Arrange
        string updatedName = "Name after update";
        string updatedDescription = "Description after update";
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto(updatedName, updatedDescription, updatedIsCompleted);
        RepositoryMock.UpdateById(Arg.Any<int>(), Arg.Any<ToDoItem>());

        // Act
        var result = Controller.UpdateById(1, toDoItemUpdateRequestDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Put_UpdateByNonExistentId_Returns404NotFound()
    {
        // Arrange
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto("Name after update", "Description after update", true);
        RepositoryMock
            .When(x => x.UpdateById(Arg.Any<int>(), Arg.Any<ToDoItem>()))
            .Do(x => throw new EntityNotFoundException(nameof(ToDoItem), 999));

        // Act
        var result = Controller.UpdateById(999, toDoItemUpdateRequestDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Put_RepositoryException_Return500InternalServerError()
    {
        // Arrange
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto("Name after update", "Description after update", true);
        RepositoryMock
                    .When(x => x.UpdateById(Arg.Any<int>(), Arg.Any<ToDoItem>()))
                    .Do(x => throw new InvalidOperationException());

        // Act
        var result = Controller.UpdateById(1, toDoItemUpdateRequestDto);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
    }
}
