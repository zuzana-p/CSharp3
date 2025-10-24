namespace ToDoList.Persistence;

using Humanizer;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsContext : DbContext // pro interakci s DB
{
    private readonly string connectionString;
    public int MaxUsedId { get; set; } // TODOzpa vymyslet lepe, docasne reseni, mit private
    public ToDoItemsContext(string connectionString = "DataSource=../../data/localdb.db", int maxUsedId = 0) // parametr = kde se nachazi DB
    {
        this.connectionString = connectionString;
        Database.Migrate();
        MaxUsedId = maxUsedId;
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => _ = optionsBuilder.UseSqlite(connectionString); // přetížení
}
