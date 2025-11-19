namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Exceptions;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController(IRepository<ToDoItem> repository) : ControllerBase
{
    private readonly IRepository<ToDoItem> repository = repository;

    [HttpPost]
    public ActionResult<ToDoItemGetResponseDto> Create(ToDoItemCreateRequestDto request)
    {
        try
        {
            var toDoItem = request.ToDomain();
            repository.Create(toDoItem);

            return CreatedAtAction(
            actionName: nameof(ReadById),
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
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
    {
        try
        {
            var toDoItems = repository.Read();
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
    public ActionResult<ToDoItemGetResponseDto> ReadById(int toDoItemId)
    {
        try
        {
            var item = repository.ReadById(toDoItemId);

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
    public IActionResult UpdateById(int todoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        try
        {
            var toDoItemValuesAfterUpdate = request.ToDomain();
            repository.UpdateById(todoItemId, toDoItemValuesAfterUpdate);
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
    public IActionResult DeleteById(int todoItemId)
    {
        try
        {
            repository.DeleteById(todoItemId);
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
