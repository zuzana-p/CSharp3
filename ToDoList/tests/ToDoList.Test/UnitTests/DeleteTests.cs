namespace ToDoList.Test.UnitTests;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Exceptions;
using ToDoList.Domain.Models;

public class DeleteTests : TestsBase
{
    [Fact]
    public void Delete_DeleteByIdValidItemId_ReturnsNoContent()
    {
        // Arrange

        // Act
        var result = Controller.DeleteById(1);

        // Assert
        RepositoryMock.Received(1).DeleteById(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.When(x => x.DeleteById(999)).Do(x => throw new EntityNotFoundException(nameof(ToDoItem), 999));

        // Act
        var result = Controller.DeleteById(999);

        // Assert
        RepositoryMock.Received(1).DeleteById(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void Delete_DeleteByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        RepositoryMock.When(x => x.DeleteById(1)).Do(x => throw new InvalidOperationException());

        // Act
        var result = Controller.DeleteById(1);

        // Assert
        RepositoryMock.Received(1).DeleteById(1);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(500);
    }
}
