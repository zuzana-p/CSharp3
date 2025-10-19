namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;

public class PostTests
{
    [Theory] // Neukazovali jsme si, ani neznam z praxe. Netuším, zda je to správně (ale funguje to). Jen jsem hledala jak pouzit parametr.
    [InlineData(false)]
    [InlineData(true)]
    public void Post_CreateItem_ReturnsCreatedAtAction(bool isCompleted)
    {
        // Arrange
        var toDoItemCreateRequestDto = new ToDoItemCreateRequestDto("Some name", "Some description", isCompleted);

        var controller = new ToDoItemsController();

        // Act
        var result = controller.Create(toDoItemCreateRequestDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("ReadById", createdAtActionResult.ActionName);

        var todoItemResponseDto = createdAtActionResult.Value as ToDoItemGetResponseDto;

        Assert.True(todoItemResponseDto.ToDoItemId > 0);
        Assert.Equal(todoItemResponseDto.ToDoItemId, createdAtActionResult.RouteValues["toDoItemId"]);

        Assert.Equal("Some name", todoItemResponseDto.Name);
        Assert.Equal("Some description", todoItemResponseDto.Description);
        Assert.Equal(isCompleted, todoItemResponseDto.IsCompleted);
    }

    [Fact]
    public void Post_TODO_Returns500InternalServerError()
    {
        // TODO: nevím jak simulovat exception
    }

}
