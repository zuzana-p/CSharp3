namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record ToDoItemGetResponseDto(int ToDoItemId, string Name, string Description, bool IsCompleted)
{
    public ToDoItemGetResponseDto(ToDoItem toDoItem)
        : this(toDoItem.ToDoItemId, toDoItem.Name, toDoItem.Description, toDoItem.IsCompleted) { }
}
