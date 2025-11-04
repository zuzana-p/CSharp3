namespace ToDoList.Test.IntegrationTests;

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
        RepositoryMock.When(x => x.Create(null)).Do(x => throw new Exception());
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
        //Assert.True(todoItemResponseDto.ToDoItemId > 0); // TODOzpa
        Assert.NotNull(createdAtActionResult.RouteValues);
        Assert.Equal(todoItemResponseDto.ToDoItemId, createdAtActionResult.RouteValues["toDoItemId"]);

        Assert.Equal(itemName, todoItemResponseDto.Name);
        Assert.Equal(itemDescription, todoItemResponseDto.Description);
        Assert.Equal(isCompleted, todoItemResponseDto.IsCompleted);
    }

    [Fact]
    public void Post_TODO_Returns500InternalServerError_NOTIMPLEMENTED()
    {
        // Arrange
        string itemName = "Name of task";
        string itemDescription = "Description of task";
        bool isCompleted = false;
        var toDoItemCreateRequestDto = new ToDoItemCreateRequestDto(itemName, itemDescription, isCompleted);

        // Act
        var result = Controller.Create(null);

        // Assert
        Assert.False(true); // TODOzpa dodelat assert
    }

}
