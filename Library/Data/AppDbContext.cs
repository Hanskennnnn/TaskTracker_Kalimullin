using Microsoft.EntityFrameworkCore;
using Library.Models;

namespace Library.Data;

public class AppDbContext : DbContext
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=tasktracker.db");
    }
}
