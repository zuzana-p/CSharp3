namespace ToDoList.Test.UnitTests;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PostTests : TestsBase
{
    public PostTests()
    {
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Post_CreateValidRequest_ReturnsCreatedAtAction_Async(bool isCompleted)
    {
        // Arrange
        string itemName = "Name of the task";
        string itemDescription = "Description of the task";
        string itemCategory = "Category of the task";
        var toDoItemCreateRequestDto = new ToDoItemCreateRequestDto(itemName, itemDescription, isCompleted, itemCategory);

        // Act
        var result = await Controller.CreateAsync(toDoItemCreateRequestDto);

        // Assert
        await RepositoryMock.Received(1).CreateAsync(Arg.Any<ToDoItem>());

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        createdAtActionResult.ActionName.Should().Be("ReadByIdAsync");

        var todoItemResponseDto = createdAtActionResult.Value as ToDoItemGetResponseDto;
        todoItemResponseDto.Should().NotBeNull();

        createdAtActionResult.RouteValues.Should().NotBeNull();
        createdAtActionResult.RouteValues["toDoItemId"].Should().Be(todoItemResponseDto.ToDoItemId);

        todoItemResponseDto.Should().BeEquivalentTo(new
        {
            Name = itemName,
            Description = itemDescription,
            Category = itemCategory,
            IsCompleted = isCompleted
        });
    }

    [Fact]
    public async Task Post_CreateUnhandledException_ReturnsInternalServerError_Async()
    {
        // Arrange
        var toDoItemCreateRequestDto = new ToDoItemCreateRequestDto("Name", "Description", false, "Category");
        RepositoryMock.When(x => x.CreateAsync(Arg.Any<ToDoItem>())).Do(_ => throw new InvalidOperationException());

        // Act
        var result = await Controller.CreateAsync(toDoItemCreateRequestDto);

        // Assert
        await RepositoryMock.Received(1).CreateAsync(Arg.Any<ToDoItem>());

        var objectResult = result.Result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(500);
    }

}
