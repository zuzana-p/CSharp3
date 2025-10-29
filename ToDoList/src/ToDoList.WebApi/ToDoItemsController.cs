namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController(ToDoItemsContext dbContext) : ControllerBase
{
    private readonly ToDoItemsContext dbContext = dbContext;

    [HttpPost]
    public ActionResult<ToDoItemGetResponseDto> Create(ToDoItemCreateRequestDto request)
    {
        try
        {
            var toDoItem = request.ToDomain();
            _ = dbContext.ToDoItems.Add(toDoItem);

            if (dbContext.SaveChanges() == 1)
            {
                return CreatedAtAction(
                    actionName: nameof(ReadById),
                    routeValues: new { toDoItemId = toDoItem.ToDoItemId },
                    value: new ToDoItemGetResponseDto(toDoItem)
                    );
            }
            else
            {
                return Problem("Problem occured during create.", null, StatusCodes.Status500InternalServerError);
            }
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
            if (!dbContext.ToDoItems.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(dbContext.ToDoItems.Select(x => new ToDoItemGetResponseDto(x)).ToList());
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{todoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> ReadById(int todoItemId)
    {
        try
        {
            var item = dbContext.ToDoItems.Find(todoItemId);

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
            var toDoItem = dbContext.ToDoItems.Find(todoItemId);

            if (toDoItem != null)
            {
                toDoItem.Name = request.Name;
                toDoItem.Description = request.Description;
                toDoItem.IsCompleted = request.IsCompleted;

                int savedRows = dbContext.SaveChanges();

                if (savedRows == 1)
                {
                    return NoContent();
                }
                else
                {
                    return Problem("Problem occured during udpate.", null, StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                return NotFound();
            }
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
            var toDoItemToDelete = dbContext.ToDoItems.Find(todoItemId);

            if (toDoItemToDelete != null)
            {
                dbContext.ToDoItems.Remove(toDoItemToDelete);

                int savedRows = dbContext.SaveChanges();

                if (savedRows == 1)
                {
                    return NoContent();
                }
                else
                {
                    return Problem("Problem occured during delete.", null, StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
}
