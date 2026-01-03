namespace ToDoList.Test.IntegrationTests;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PutTests : TestsBase
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task UpdateById_ExistingId_UpdatesAndReturnsNoContent_Async(bool updatedIsCompleted)
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Task to be updated 1",
            Description = "Description to be updated 1",
            Category = "Category 1",
            IsCompleted = false,
        };
        var toDoItem2 = new ToDoItem
        {
            Name = "Task to be updated 2",
            Description = "Description to be updated 2",
            Category = "Category 2",
            IsCompleted = true
        };
        await DbContext.ToDoItems.AddRangeAsync(toDoItem1, toDoItem2);
        await DbContext.SaveChangesAsync();

        string updatedName = "Name after update";
        string updatedDescription = "Description after update";
        string updatedCategory = "Category after update";
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto(updatedName, updatedDescription, updatedIsCompleted, updatedCategory);

        // Act
        var result = await Controller.UpdateByIdAsync(toDoItem1.ToDoItemId, toDoItemUpdateRequestDto);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        var itemsIds = DbContext.ToDoItems.Select(x => x.ToDoItemId);
        itemsIds.Should().Contain(toDoItem1.ToDoItemId)
            .And.Contain(toDoItem2.ToDoItemId);

        var updatedItem = await DbContext.ToDoItems.FindAsync(toDoItem1.ToDoItemId);
        updatedItem.Should().NotBeNull();
        updatedItem.Name.Should().Be(updatedName);
        updatedItem.Description.Should().Be(updatedDescription);
        updatedItem.Category.Should().Be(updatedCategory);
        updatedItem.IsCompleted.Should().Be(updatedIsCompleted);

        var notUpdatedItem = await DbContext.ToDoItems.FindAsync(toDoItem2.ToDoItemId);
        notUpdatedItem.Should().NotBeNull();
        notUpdatedItem.Should().Be(toDoItem2);
    }

    [Fact]
    public async Task Put_UpdateByNonExistentId_Returns404NotFound_Async()
    {
        // Arrange
        var toDoItem = new ToDoItem
        {
            Name = "Name not to be updated",
            Description = "Description not to be updated",
            Category = "Category",
            IsCompleted = false
        };
        await DbContext.ToDoItems.AddAsync(toDoItem);
        await DbContext.SaveChangesAsync();
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto("Name after update", "Description after update", true, "Category after update");

        // Act
        var result = await Controller.UpdateByIdAsync(9999, toDoItemUpdateRequestDto);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
