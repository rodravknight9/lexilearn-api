using Lexilearn.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Lexilearn.Identity.Persistence;

public class LexilearnIdentityDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public LexilearnIdentityDbContext(DbContextOptions<LexilearnIdentityDbContext> dbContextOptions) 
        : base(dbContextOptions)
    {
    }
}