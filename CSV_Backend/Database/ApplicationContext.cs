using CSV_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace CSV_Backend.Database;

public class ApplicationContext : DbContext
{
    private DbSet<Person> Persons { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
}