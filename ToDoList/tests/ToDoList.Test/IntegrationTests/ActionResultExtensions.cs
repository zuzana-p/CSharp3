namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;

public static class ActionResultExtensions
{
    public static T? GetValue<T>(this ActionResult<T> result) => result.Result is null
        ? result.Value
        : (T?)(result.Result as ObjectResult)?.Value;
    // toto z hodiny mi z nějakého důvodu nefungovalo. Nechtěla jsem na tom zabít čas, tak jsem to s pomocí chatGPT vyřešila jinak a řešila testy. Zkusím se k tomu ještě vrátit.

    public static object? GetValue(this IActionResult result) => (result as ObjectResult)?.Value;
}
