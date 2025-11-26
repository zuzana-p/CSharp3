namespace ToDoList.Domain.DTOs;

using System.Text.Json.Serialization;
using ToDoList.Domain.Models;

public record ToDoItemGetResponseDto
{
    public int ToDoItemId { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public bool IsCompleted { get; init; }
    public string? Category { get; init; }

    // mark the constructor System.Text.Json should use
    [JsonConstructor]
    public ToDoItemGetResponseDto(int toDoItemId, string name, string? description, bool isCompleted, string? category)
    {
        ToDoItemId = toDoItemId;
        Name = name;
        Description = description;
        IsCompleted = isCompleted;
        Category = category;
    }
    public ToDoItemGetResponseDto(ToDoItem toDoItem)
        : this(toDoItem.ToDoItemId, toDoItem.Name, toDoItem?.Description, toDoItem.IsCompleted, toDoItem?.Category) { } // todoZPA doresit warning - dotaz na Lubose
}
