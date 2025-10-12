namespace ToDoList.WebApi;

using System.IO.Compression;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private static List<ToDoItem> items = [];

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) //pouzijeme DTO - Data Transfer Object
    {
        try
        {
            var toDoItem = request.ToDomain();
            toDoItem.ToDoItemId = items.Count != 0 ? items.Max(x => x.ToDoItemId) : 1;
            items.Add(toDoItem);
            return CreatedAtAction(nameof(ReadById), toDoItem.ToDoItemId); //TODO Extra (nepovinné): Použij CreatedAtAction, abys vrátila vytvořený předmět společně s cestou kde se dá najít a s jeho ID.
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
            if (items == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(items.Select(x => new ToDoItemGetResponseDto(x)).ToList()); // TODO lepsi pres konstruktor primo z ToDoItem
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
            var item = items.Find(x => x.ToDoItemId == todoItemId); // Q: Z toho co jsem dohledala Find není z LINQ. Ale podle zadání úkolu je. Jak to prosím je?

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
            if (items.Any(x => x.ToDoItemId == todoItemId))
            {
                int indexOfOriginalToDoItem = items.FindIndex(x => x.ToDoItemId == todoItemId);
                // Q: Nemůže se mi index pod rukama změnit? Např. při přístupu více lidí. (Následuji zadání k úkolu, proto přes FindIndex).

                var updatedToDoItem = request.ToDomain();
                items[indexOfOriginalToDoItem] = updatedToDoItem;
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
            var toDoItemToDelete = items.Find(x => x.ToDoItemId == todoItemId);

            if (toDoItemToDelete != null) // Find, protože následuji zadání k úkolu
            {
                if (items.Remove(toDoItemToDelete))
                {
                    return NoContent();
                }
                else
                {
                    return Problem("Problem occured during deletion}.", null, StatusCodes.Status500InternalServerError);
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
