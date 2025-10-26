namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class PostTests : TestsBase
{
    [Theory] // Neukazovali jsme si, ani neznam z praxe. Netuším, zda je to správně (ale funguje to). Jen jsem hledala jak pouzit parametr.
    [InlineData(false)]
    [InlineData(true)]
    public void Post_CreateItem_ReturnsCreatedAtAction(bool isCompleted)
    {
        // Arrange
        var toDoItemCreateRequestDto = new ToDoItemCreateRequestDto("Some name", "Some description", isCompleted);

        // Act
        var result = Controller.Create(toDoItemCreateRequestDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("ReadById", createdAtActionResult.ActionName);

        var todoItemResponseDto = createdAtActionResult.Value as ToDoItemGetResponseDto;

        Assert.NotNull(todoItemResponseDto);
        Assert.True(todoItemResponseDto.ToDoItemId > 0);
        Assert.NotNull(createdAtActionResult.RouteValues);
        Assert.Equal(todoItemResponseDto.ToDoItemId, createdAtActionResult.RouteValues["toDoItemId"]);

        Assert.Equal("Some name", todoItemResponseDto.Name);
        Assert.Equal("Some description", todoItemResponseDto.Description);
        Assert.Equal(isCompleted, todoItemResponseDto.IsCompleted);
    }

    [Fact]
    public void Post_TODO_Returns500InternalServerError_NOTIMPLEMENTED()
    {
        throw new NotImplementedException();
    }

}
