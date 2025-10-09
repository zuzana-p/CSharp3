namespace ToDoList.WebApi;

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
            return Ok(StatusCodes.Status201Created); //TODO Extra (nepovinné): Použij CreatedAtAction, abys vrátila vytvořený předmět společně s cestou kde se dá najít a s jeho ID.
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public IActionResult Read()
    {
    }

    [HttpGet("{todoItemId:int}")]
    public IActionResult ReadById(int todoItemId)
    {
        try
        {
            throw new Exception("Neco se fakt nepovedlo.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return Ok();
    }

    [HttpPut("{todoItemId:int}")]
    public IActionResult UpdateById(int todoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        try
        {
            throw new Exception("Neco se fakt nepovedlo.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return Ok();
    }

    [HttpDelete("{todoItemId:int}")]
    public IActionResult DeleteById(int todoItemId)
    {
        return Ok();
    }

}
