namespace ToDoList.Test.UnitTests;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Exceptions;
using ToDoList.Domain.Models;

public class DeleteTests : TestsBase
{
    [Fact]
    public async Task Delete_DeleteByIdValidItemId_ReturnsNoContent_Async()
    {
        // Arrange

        // Act
        var result = await Controller.DeleteByIdAsync(1);

        // Assert
        await RepositoryMock.Received(1).DeleteByIdAsync(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_DeleteByIdInvalidItemId_ReturnsNotFound_Async()
    {
        // Arrange
        RepositoryMock.When(x => x.DeleteByIdAsync(999)).Do(x => throw new EntityNotFoundException(nameof(ToDoItem), 999));

        // Act
        var result = await Controller.DeleteByIdAsync(999);

        // Assert
        await RepositoryMock.Received(1).DeleteByIdAsync(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_DeleteByIdUnhandledException_ReturnsInternalServerError_Async()
    {
        // Arrange
        RepositoryMock.When(x => x.DeleteByIdAsync(1)).Do(x => throw new InvalidOperationException());

        // Act
        var result = await Controller.DeleteByIdAsync(1);

        // Assert
        await RepositoryMock.Received(1).DeleteByIdAsync(1);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(500);
    }
}
