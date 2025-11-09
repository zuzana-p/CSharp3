namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Exceptions;
using ToDoList.Domain.Models;

public class DeleteTests : TestsBase
{
    [Fact]
    public void DeleteDeleteById_ExistingId_DeletesItem()
    {
        // Arrange
        RepositoryMock.DeleteById(Arg.Any<int>());

        // Act
        var result = Controller.DeleteById(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_NonExistentId_Returns404NotFound()
    {
        // Arrange
        RepositoryMock.When(x => x.DeleteById(Arg.Any<int>())).Do(x => throw new EntityNotFoundException(nameof(ToDoItem), 999));

        // Act
        var result = Controller.DeleteById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_RepositoryException_Returns500InternalServerError()
    {
        // Arrange
        RepositoryMock.When(x => x.DeleteById(Arg.Any<int>())).Do(x => throw new InvalidOperationException());

        // Act
        var result = Controller.DeleteById(1);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
    }
}
