using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContext>();
    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>(); // kdykoliv se odkazujina IRepository<ToDoItem>, tak pouzij ToDoItemRepository ~~ Dependency Injection
}

var app = builder.Build();
{
    app.MapControllers();
}

app.Run();
