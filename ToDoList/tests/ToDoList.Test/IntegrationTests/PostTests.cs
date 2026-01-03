namespace ToDoList.Test.IntegrationTests;

using FluentAssertions;
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
        var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAtActionResult.ActionName.Should().Be(nameof(Controller.ReadByIdAsync));

        var todoItemResponseDto = createdAtActionResult.Value as ToDoItemGetResponseDto;
        todoItemResponseDto.Should().NotBeNull();
        todoItemResponseDto.ToDoItemId.Should().BeGreaterThan(0);
        createdAtActionResult.RouteValues.Should().NotBeNull();
        createdAtActionResult.RouteValues["toDoItemId"].Should().Be(todoItemResponseDto.ToDoItemId);

        todoItemResponseDto.Name.Should().Be(itemName);
        todoItemResponseDto.Description.Should().Be(itemDescription);
        todoItemResponseDto.Category.Should().Be(itemCategory);
        todoItemResponseDto.IsCompleted.Should().Be(isCompleted);
    }
}
