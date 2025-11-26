namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class PostTests : TestsBase
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Post_CreateItem_ReturnsCreatedAtAction_Async(bool isCompleted)
    {
        // Arrange
        string itemName = "Name of task";
        string itemDescription = "Description of task";
        string itemCategory = "Category of task";
        var toDoItemCreateRequestDto = new ToDoItemCreateRequestDto(itemName, itemDescription, isCompleted, itemCategory);

        // Act
        var result = await Controller.CreateAsync(toDoItemCreateRequestDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("ReadByIdAsync", createdAtActionResult.ActionName);

        var todoItemResponseDto = createdAtActionResult.Value as ToDoItemGetResponseDto;
        Assert.NotNull(todoItemResponseDto);
        Assert.True(todoItemResponseDto.ToDoItemId > 0);
        Assert.NotNull(createdAtActionResult.RouteValues);
        Assert.Equal(todoItemResponseDto.ToDoItemId, createdAtActionResult.RouteValues["toDoItemId"]);

        Assert.Equal(itemName, todoItemResponseDto.Name);
        Assert.Equal(itemDescription, todoItemResponseDto.Description);
        Assert.Equal(isCompleted, todoItemResponseDto.IsCompleted);
        Assert.Equal(itemCategory, todoItemResponseDto.Category);
    }
}
