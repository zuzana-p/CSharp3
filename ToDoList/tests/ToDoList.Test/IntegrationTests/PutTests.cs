namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PutTests : TestsBase
{
    // Update tests have itemId = 3x
    [Theory] // Neukazovali jsme si, ani neznam z praxe. Netuším, zda je to správně (ale funguje to). Jen jsem hledala jak pouzit parametr.
    [InlineData(false)]
    [InlineData(true)]
    public void Put_UpdateById_UpdatesCorrectlyOnlyGivenItem(bool updatedIsCompleted)
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 31,
            Name = "Some name 1",
            Description = "Some description 1",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 32,
            Name = "Some name 2",
            Description = "Some description 2",
            IsCompleted = true
        };
        Controller.AddItemToStorage(todoItem1);
        Controller.AddItemToStorage(todoItem2);

        string updatedName = "Some updated name 1";
        string updatedDescription = "Some updated description 1";
        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto(updatedName, updatedDescription, updatedIsCompleted);

        // Act
        var result = Controller.UpdateById(todoItem1.ToDoItemId, toDoItemUpdateRequestDto);

        // Assert
        _ = Assert.IsType<NoContentResult>(result);

        var items = Controller.GetAllItems();
        var itemsIds = items.Select(x => x.ToDoItemId);
        Assert.True(itemsIds.Count() == 2 && itemsIds.Contains(31) && itemsIds.Contains(32));
        // Kontroluji, že se mi nezmění množina ids (myslím, že se mi to původně při implementaci stalo). Podle id potom přistupuji k itemům v dalším assertu.

        var updatedItem = items.First(x => x.ToDoItemId == todoItem1.ToDoItemId);
        Assert.Equal(updatedName, updatedItem.Name);
        Assert.Equal(updatedDescription, updatedItem.Description);
        Assert.Equal(updatedIsCompleted, updatedItem.IsCompleted);

        var notUpdatedItem = items.First(x => x.ToDoItemId == todoItem2.ToDoItemId);
        Assert.Equal(todoItem2.Name, notUpdatedItem.Name);
        Assert.Equal(todoItem2.Description, notUpdatedItem.Description);
        Assert.Equal(todoItem2.IsCompleted, notUpdatedItem.IsCompleted);
    }

    [Fact]
    public void Put_UpdateByNonExistentId_Returns404NotFound()
    {
        // Arrange
        var todoItem = new ToDoItem
        {
            ToDoItemId = 31,
            Name = "Some name",
            Description = "Some description",
            IsCompleted = false
        };
        Controller.AddItemToStorage(todoItem);

        var toDoItemUpdateRequestDto = new ToDoItemUpdateRequestDto("Some updated name", "Some updated description", true);

        // Act
        var result = Controller.UpdateById(32, toDoItemUpdateRequestDto);

        // Assert
        _ = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Put_TODO_Return500InternalServerError_NOTIMPLEMENTED() => throw new NotImplementedException();
}
