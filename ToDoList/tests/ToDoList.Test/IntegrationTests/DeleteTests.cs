namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;

public class DeleteTests : TestsBase
{
    [Fact]
    public void DeleteDeleteById_ExistingId_ReturnsNoContent() // TODOzpa jak overit, ze nesmazu nic jineho?
    {
        // Arrange
        var itemToDelete = new ToDoItem
        {
            Name = "Task to be deleted",
            Description = "This task will be deleted",
            IsCompleted = false
        };
        _ = DbContext.ToDoItems.Add(itemToDelete);
        _ = DbContext.SaveChanges();

        // Act
        var result = Controller.DeleteById(itemToDelete.ToDoItemId);

        // Assert
        _ = Assert.IsType<NoContentResult>(result);
        Assert.Null(DbContext.ToDoItems.Find(itemToDelete.ToDoItemId));
    }

    [Fact]
    public void Delete_NonExistentId_Returns404NotFound() // TODOzpa jak overit, ze nesmazu nic jineho?
    {
        // Arrange
        var itemToDelete = new ToDoItem
        {
            Name = "Task not to be deleted",
            Description = "This task will not be deleted",
            IsCompleted = true
        };
        _ = DbContext.ToDoItems.Add(itemToDelete);
        _ = DbContext.SaveChanges();

        // Act
        var result = Controller.DeleteById(ArbitraryNonExistentId);

        // Assert
        _ = Assert.IsType<NotFoundResult>(result);
    }

    // [Fact]
    // public void Delete_RemoveUnsuccesful_Returns500InternalServerError_NOTIMPLEMENTED() => throw new NotImplementedException();
}
