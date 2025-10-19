namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class DeleteTests
{
    [Fact]
    public void Delete_DeleteById_RemovesExactlyTheGivenItem()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Some name 1",
            Description = "Some description 1",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Some name 2",
            Description = "Some description 2",
            IsCompleted = true
        };
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(todoItem1);
        controller.AddItemToStorage(todoItem2);

        // Act
        var result = controller.DeleteById(todoItem1.ToDoItemId);

        // Assert
        _ = Assert.IsType<NoContentResult>(result);

        var items = controller.GetAllItems();
        var notDeletedItem = Assert.Single(items);
        Assert.Equal(2, notDeletedItem.ToDoItemId);
        Assert.Equal(todoItem2.Name, notDeletedItem.Name);
        Assert.Equal(todoItem2.Description, notDeletedItem.Description);
        Assert.Equal(todoItem2.IsCompleted, notDeletedItem.IsCompleted);
    }

    [Fact]
    public void Delete_NonExistentId_Returns404NotFound()
    {
        // Arrange
        var todoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Some name",
            Description = "Some description",
            IsCompleted = false
        };

        var controller = new ToDoItemsController();
        controller.AddItemToStorage(todoItem);

        // Act
        var result = controller.DeleteById(2);

        // Assert
        _ = Assert.IsType<NotFoundResult>(result);

        var items = controller.GetAllItems();
        var notDeletedItem = Assert.Single(items);
        Assert.Equal(1, notDeletedItem.ToDoItemId);
        Assert.Equal(todoItem.Name, notDeletedItem.Name);
        Assert.Equal(todoItem.Description, notDeletedItem.Description);
        Assert.Equal(todoItem.IsCompleted, notDeletedItem.IsCompleted);
    }

    [Fact]
    public void Delete_NullItems_Returns500InternalServerError()
    {
        //Arrange
        var controller = new ToDoItemsController(null as List<ToDoItem>);

        //Act
        var objectResult = controller.DeleteById(1) as ObjectResult;

        //Assert
        Assert.Equal(500, objectResult.StatusCode);
    }

    public void Delete_RemoveUnsuccesful_Returns500InternalServerError()
    {
        // TODO: nevím jak simulovat exception
    }

}
