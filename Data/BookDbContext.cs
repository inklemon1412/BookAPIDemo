using Microsoft.EntityFrameworkCore;
using BookAPIDemo.Items;

namespace BookAPIDemo.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
            
        }

        public DbSet<Book> Book {  get; set; }

        public DbSet<Author> Author { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
