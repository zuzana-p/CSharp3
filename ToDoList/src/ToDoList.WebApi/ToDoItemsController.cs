namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Exceptions;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController(IRepositoryAsync<ToDoItem> repository) : ControllerBase
{
    private readonly IRepositoryAsync<ToDoItem> repository = repository;

    [HttpPost]
    public async Task<ActionResult<ToDoItemGetResponseDto>> CreateAsync(ToDoItemCreateRequestDto request)
    {
        try
        {
            var toDoItem = request.ToDomain();
            await repository.CreateAsync(toDoItem);

            return CreatedAtAction(
            actionName: nameof(ReadByIdAsync),
            routeValues: new { toDoItemId = toDoItem.ToDoItemId },
            value: new ToDoItemGetResponseDto(toDoItem)
            );
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemGetResponseDto>>> ReadAsync()
    {
        try
        {
            var toDoItems = await repository.ReadAsync();
            if (!toDoItems.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(toDoItems.Select(x => new ToDoItemGetResponseDto(x)).ToList());
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{todoItemId:int}")]
    public async Task<ActionResult<ToDoItemGetResponseDto>> ReadByIdAsync(int toDoItemId)
    {
        try
        {
            var item = await repository.ReadByIdAsync(toDoItemId);

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new ToDoItemGetResponseDto(item));
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{todoItemId:int}")]
    public async Task<IActionResult> UpdateByIdAsync(int todoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        try
        {
            var toDoItemValuesAfterUpdate = request.ToDomain();
            await repository.UpdateByIdAsync(todoItemId, toDoItemValuesAfterUpdate);
            return NoContent();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }


    [HttpDelete("{todoItemId:int}")]
    public async Task<IActionResult> DeleteByIdAsync(int todoItemId)
    {
        try
        {
            await repository.DeleteByIdAsync(todoItemId);
            return NoContent();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
}
