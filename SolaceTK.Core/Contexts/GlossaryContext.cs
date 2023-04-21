using Microsoft.EntityFrameworkCore;

namespace SolaceTK.Core.Contexts
{
    public class GlossaryContext : DbContext
    {

        //public DbSet<GlossaryIndex> Index { get; set; }

        // Default Constructor:
        public GlossaryContext(DbContextOptions<GlossaryContext> options) : base(options)
        {

        }

    }
}
