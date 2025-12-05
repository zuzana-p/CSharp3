namespace ToDoList.Frontend.Models;

public record ToDoItemView
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required bool IsCompleted { get; set; }
    public string? Category { get; set; }
}
