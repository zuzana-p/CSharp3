namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsContext : DbContext // pro interakci s DB
{
    private readonly string connectionString;
    public ToDoItemsContext(string connectionString = "DataSource=../../data/localdb.db", int maxUsedId = 0) // parametr = kde se nachazi DB
    {
        this.connectionString = connectionString;
        Database.Migrate();
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => _ = optionsBuilder.UseSqlite(connectionString); // přetížení
}
