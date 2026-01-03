namespace ToDoList.Frontend.Clients;

using System.Net;
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
            ArgumentNullException.ThrowIfNull(response);


            toDoItemViews = [.. response.Select(dto => new ToDoItemView()
            {
                Id = dto.ToDoItemId,
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                IsCompleted = dto.IsCompleted
            }
            )];
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            // empty db, return empty list
        }
        return toDoItemViews;
    }

    public async Task<ToDoItemView> ReadItemByIdAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{id}");
        ArgumentNullException.ThrowIfNull(response);

        var toDoItem = new ToDoItemView()
        {
            Id = response.ToDoItemId,
            Name = response.Name,
            Description = response.Description,
            Category = response.Category,
            IsCompleted = response.IsCompleted
        };

        return toDoItem;
    }

    public async Task UpdateItemAsync(ToDoItemView toDoItemView)
    {
        var itemRequest = new ToDoItemUpdateRequestDto(toDoItemView.Name, toDoItemView.Description, toDoItemView.IsCompleted, toDoItemView.Category);
        var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{toDoItemView.Id}", itemRequest);

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new InvalidOperationException($"Item {toDoItemView.Id} no longer exists.");
        }

        else
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Update failed with status {(int)response.StatusCode}: {content}",
                null,
                response.StatusCode);
        }
    }

    public async Task DeleteItemByIdAsync(ToDoItemView toDoItemView)
    {
        var response = await httpClient.DeleteAsync($"api/ToDoItems/{toDoItemView.Id}");

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new InvalidOperationException($"Item {toDoItemView.Id} no longer exists.");
        }

        else
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Update failed with status {(int)response.StatusCode}: {content}",
                null,
                response.StatusCode);
        }
    }
}
