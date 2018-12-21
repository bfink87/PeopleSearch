using Microsoft.EntityFrameworkCore;
using PeopleSearch.Models;

namespace PeopleSearch.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options){}

        public DbSet<Person> People { get; set; }
    }
}
