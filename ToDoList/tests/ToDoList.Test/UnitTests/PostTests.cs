namespace ToDoList.Test.UnitTests;

using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

public class PostTests : TestsBase
{
    public PostTests()
    {
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Post_CreateItem_ReturnsCreatedAtAction(bool isCompleted)
    {
        // Arrange
        string itemName = "Name of task";
        string itemDescription = "Description of task";
        var toDoItemCreateRequestDto = new ToDoItemCreateRequestDto(itemName, itemDescription, isCompleted);

        // Act
        var result = Controller.Create(toDoItemCreateRequestDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("ReadById", createdAtActionResult.ActionName);

        var todoItemResponseDto = createdAtActionResult.Value as ToDoItemGetResponseDto;
        Assert.NotNull(todoItemResponseDto);
        Assert.NotNull(createdAtActionResult.RouteValues);
        Assert.Equal(todoItemResponseDto.ToDoItemId, createdAtActionResult.RouteValues["toDoItemId"]);

        Assert.Equal(itemName, todoItemResponseDto.Name);
        Assert.Equal(itemDescription, todoItemResponseDto.Description);
        Assert.Equal(isCompleted, todoItemResponseDto.IsCompleted);
    }

    [Fact]
    public void Post_RepositoryException_Returns500InternalServerError()
    {
        // Arrange
        var toDoItemCreateRequestDto = new ToDoItemCreateRequestDto("Name", "Description", false);
        RepositoryMock.When(x => x.Create(Arg.Any<ToDoItem>())).Do(_ => throw new InvalidOperationException());

        // Act
        var result = Controller.Create(toDoItemCreateRequestDto);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
    }

}
