namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class PostTests : TestsBase
{
    [Theory] // Neukazovali jsme si, ani neznam z praxe. Netuším, zda je to správně (ale funguje to). Jen jsem hledala jak pouzit parametr.
    [InlineData(false)]
    [InlineData(true)]
    public void Post_CreateItem_ReturnsCreatedAtAction(bool isCompleted) // TODOzpa prejmenovat
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
        Assert.True(todoItemResponseDto.ToDoItemId > 0);
        Assert.NotNull(createdAtActionResult.RouteValues);
        Assert.Equal(todoItemResponseDto.ToDoItemId, createdAtActionResult.RouteValues["toDoItemId"]);

        Assert.Equal(itemName, todoItemResponseDto.Name);
        Assert.Equal(itemDescription, todoItemResponseDto.Description);
        Assert.Equal(isCompleted, todoItemResponseDto.IsCompleted);
    }

    // [Fact]
    // public void Post_TODO_Returns500InternalServerError_NOTIMPLEMENTED() => throw new NotImplementedException();

}
