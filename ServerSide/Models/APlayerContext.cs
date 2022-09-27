using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;


namespace ServerSide.Models
{
    public class APlayerContext: DbContext
    {
        public APlayerContext(DbContextOptions<APlayerContext> options)
           : base(options)
        {
        }

        public DbSet<APlayerContext> APlayerItems { get; set; } = null!;
    }
}

