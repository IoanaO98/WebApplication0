using Microsoft.EntityFrameworkCore;

namespace WebApplication0.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
        : base(options)
        {
        }

        public DbSet<ToDoItems> ToDoItems { get; set; } = null!;
    }
}
