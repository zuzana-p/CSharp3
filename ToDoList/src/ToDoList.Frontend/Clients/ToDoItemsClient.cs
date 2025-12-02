namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Models;

public class ToDoItemsClient(HttpClient httpClient) : IToDoItemsClient
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var toDoItemViews = new List<ToDoItemView>();

        try
        {
            var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");

            toDoItemViews = [.. response.Select(dto => new ToDoItemView()
            {
                Id = dto.ToDoItemId,
                Name = dto.Name,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted
            }
            )];
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // empty db
        }
        return toDoItemViews;
    }

    public async Task<ToDoItemView> ReadItemByIdAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{id}");

        var toDoItem = new ToDoItemView()
        {
            Id = response.ToDoItemId,
            Name = response.Name,
            Description = response.Description,
            IsCompleted = response.IsCompleted
        };

        return toDoItem;
    }

    public async Task UpdateItemAsync(ToDoItemView toDoItem)
    {
        var itemRequest = new ToDoItemUpdateRequestDto(toDoItem.Name, toDoItem.Description, toDoItem.IsCompleted, toDoItem.Category); // todoZPA trycatch + category
        await httpClient.PutAsJsonAsync($"api/ToDoItems/{toDoItem.Id}", itemRequest);
    }

    public async Task DeleteItemByIdAsync(ToDoItemView toDoItem)
    {
        // todoZPA - try catch
        var response = await httpClient.DeleteAsync($"api/ToDoItems/{toDoItem.Id}");
    }

}
