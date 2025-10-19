namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly List<ToDoItem> _items;

    public ToDoItemsController()
    {
        _items = new List<ToDoItem>();
    }

    public ToDoItemsController(List<ToDoItem> items)
    {
        _items = items;
    }

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request)
    {
        try
        {
            var toDoItem = request.ToDomain();
            toDoItem.ToDoItemId = _items.Count != 0 ? _items.Max(x => x.ToDoItemId) + 1 : 1;
            _items.Add(toDoItem);
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
    public IActionResult Read()
    {
        try
        {
            if (_items == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(_items.Select(x => new ToDoItemGetResponseDto(x)).ToList());
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{todoItemId:int}")]
    public IActionResult ReadById(int todoItemId)
    {
        try
        {
            var item = _items.Find(x => x.ToDoItemId == todoItemId); // Q: Z toho co jsem dohledala Find není z LINQ. Ale podle zadání úkolu je. Jak to prosím je?

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
            if (_items.Any(x => x.ToDoItemId == todoItemId))
            {
                int indexOfOriginalToDoItem = _items.FindIndex(x => x.ToDoItemId == todoItemId);
                // Q: Nemůže se mi index pod rukama změnit? Např. při přístupu více lidí. (Následuji zadání k úkolu, proto přes FindIndex).

                var updatedToDoItem = request.ToDomain();
                updatedToDoItem.ToDoItemId = todoItemId;

                _items[indexOfOriginalToDoItem] = updatedToDoItem;
                return NoContent();
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
            var toDoItemToDelete = _items.Find(x => x.ToDoItemId == todoItemId);

            if (toDoItemToDelete != null) // Find, protože následuji zadání k úkolu
            {
                if (_items.Remove(toDoItemToDelete))
                {
                    return NoContent();
                }
                else
                {
                    return Problem("Problem occured during deletion.", null, StatusCodes.Status500InternalServerError);
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

    public void AddItemToStorage(ToDoItem item)
    {
        _items.Add(item);
    }

    public List<ToDoItem> GetAllItems()
    {
        return _items;
    }

}
