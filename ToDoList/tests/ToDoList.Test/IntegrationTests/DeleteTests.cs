namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;

public class DeleteTests : TestsBase
{
    // Delete tests have itemId = 4x
    [Fact]
    public void Delete_DeleteById_RemovesExactlyTheGivenItem()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 41,
            Name = "Some name 1",
            Description = "Some description 1",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 42,
            Name = "Some name 2",
            Description = "Some description 2",
            IsCompleted = true
        };

        Controller.AddItemToStorage(todoItem1);
        Controller.AddItemToStorage(todoItem2);

        // Act
        var result = Controller.DeleteById(todoItem1.ToDoItemId);

        // Assert
        _ = Assert.IsType<NoContentResult>(result);

        var notDeletedItems = Controller.GetAllItems();
        Assert.Null(notDeletedItems.Find(todoItem1.ToDoItemId));
        Assert.NotNull(notDeletedItems.Find(todoItem2.ToDoItemId));
    }


    [Fact]
    public void Delete_NonExistentId_Returns404NotFound()
    {
        // Arrange
        var todoItem = new ToDoItem
        {
            ToDoItemId = 41,
            Name = "Some name",
            Description = "Some description",
            IsCompleted = false
        };

        Controller.AddItemToStorage(todoItem);

        // Act
        var result = Controller.DeleteById(42);

        // Assert
        _ = Assert.IsType<NotFoundResult>(result);

        var items = Controller.GetAllItems();
        var notDeletedItem = items.Find(todoItem.ToDoItemId);
        Assert.NotNull(notDeletedItem);
        Assert.Equal(todoItem.Name, notDeletedItem.Name);
        Assert.Equal(todoItem.Description, notDeletedItem.Description);
        Assert.Equal(todoItem.IsCompleted, notDeletedItem.IsCompleted);
    }

    [Fact]
    public void Delete_NullItems_Returns404NotFound()
    {
        //Arrange

        //Act
        var result = Controller.DeleteById(41);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_RemoveUnsuccesful_Returns500InternalServerError_NOTIMPLEMENTED() => throw new NotImplementedException();
}
