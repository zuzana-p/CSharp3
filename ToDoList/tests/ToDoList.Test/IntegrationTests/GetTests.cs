namespace ToDoList.Test.IntegrationTests;


public class GetTests : TestsBase
{
    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Name of task 1",
            Description = "Description 1",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            Name = "Name of task 2",
            Description = "Description 2",
            IsCompleted = true
        };
        DbContext.ToDoItems.AddRange(toDoItem1, toDoItem2);
        DbContext.SaveChanges();

        // Act
        var result = Controller.Read();

        // Assert
        var dtoResult = Assert.IsType<List<ToDoItemGetResponseDto>>(result.GetValue());

        var returnedToDoItem1 = dtoResult.Find(x => x.ToDoItemId == toDoItem1.ToDoItemId);
        Assert.NotNull(returnedToDoItem1);
        Assert.Equal(toDoItem1.ToDoItemId, returnedToDoItem1.ToDoItemId);
        Assert.Equal(toDoItem1.Name, returnedToDoItem1.Name);
        Assert.Equal(toDoItem1.Description, returnedToDoItem1.Description);
        Assert.Equal(toDoItem1.IsCompleted, returnedToDoItem1.IsCompleted);

        var returnedToDoItem2 = dtoResult.Find(x => x.ToDoItemId == toDoItem2.ToDoItemId);
        Assert.NotNull(returnedToDoItem2);
        Assert.Equal(toDoItem2.ToDoItemId, returnedToDoItem2.ToDoItemId);
        Assert.Equal(toDoItem2.Name, returnedToDoItem2.Name);
        Assert.Equal(toDoItem2.Description, returnedToDoItem2.Description);
        Assert.Equal(toDoItem2.IsCompleted, returnedToDoItem2.IsCompleted);
    }

    // [Fact]
    // public void Get_NullItems_Returns404NotFound_NOTIMPLEMENTED() => throw new NotImplementedException();

    // [Fact]
    // public void Get_TODO_Returns500InternalServerError_NOTIMPLEMENTED() => throw new NotImplementedException();

    [Fact]
    public void GetById_ItemId_ReturnsItem()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Name of task 1",
            Description = "Description 1",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            Name = "Name of task 2",
            Description = "Description 2",
            IsCompleted = true
        };
        DbContext.ToDoItems.AddRange(toDoItem1, toDoItem2);
        DbContext.SaveChanges();

        // Act
        var result = Controller.ReadById(toDoItem1.ToDoItemId);

        // Assert
        var dtoResult = Assert.IsType<ToDoItemGetResponseDto>(result.GetValue());
        Assert.Equal(toDoItem1.ToDoItemId, dtoResult.ToDoItemId);
        Assert.Equal(toDoItem1.Name, dtoResult.Name);
        Assert.Equal(toDoItem1.Description, dtoResult.Description);
        Assert.Equal(toDoItem1.IsCompleted, dtoResult.IsCompleted);
    }

    [Fact]
    public void GetById_NonExistenstId_Returns404NotFound()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            Name = "Name of task 1",
            Description = "Description 1",
            IsCompleted = false
        };
        DbContext.ToDoItems.Add(toDoItem1);
        DbContext.SaveChanges();

        // Act
        var result = Controller.ReadById(9999); // 9999 = nonexistent ID

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}
