using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    // Configure DI Container
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<ToDoItemsContext>();

    builder.Services.AddScoped<IRepositoryAsync<ToDoItem>, ToDoItemsRepository>(); // Dependency Injection
}

var app = builder.Build();

using (var scope = app.Services.CreateScope()) // neslo mi provest migraci, s pomoci chatGPT jsem se dobrala k tomu, že problém byl v migraci v konstruktoru ToDoItemsContextu
{
    var db = scope.ServiceProvider.GetRequiredService<ToDoItemsContext>();
    db.Database.Migrate();
}

{
    // Configure Middleware
    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI(config => config.SwaggerEndpoint("v1/swagger.json", "ToDoList API V1"));
}

app.Run();
