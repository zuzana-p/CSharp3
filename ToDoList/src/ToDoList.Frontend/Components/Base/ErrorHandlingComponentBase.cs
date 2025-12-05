namespace ToDoList.Frontend.Components.Base;

using Microsoft.AspNetCore.Components;

public abstract class ErrorHandlingComponentBase : ComponentBase
{
    protected bool ShowErrorModal { get; set; }
    protected string? ErrorMessage { get; private set; }

    protected async Task<bool> ExecuteWithErrorHandlingAsync(Func<Task> operation)
    {
        try
        {
            await operation();
            //return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Unexpected error: {ex.Message}";
        }

        ShowErrorModal = true;
        return false;
    }

    public void HandleModalClose() => ShowErrorModal = false;
}
