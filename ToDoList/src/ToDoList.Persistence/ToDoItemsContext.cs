namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsContext(string connectionString = "DataSource=../../data/localdb.db") : DbContext
{
    private readonly string connectionString = connectionString;

    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => _ = optionsBuilder.UseSqlite(connectionString);
}
