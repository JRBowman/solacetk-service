using Microsoft.EntityFrameworkCore;
using SolaceTK.Models;

namespace SolaceTK.Data.Contexts
{
    public class ControllerContext : DbContext
    {
        public DbSet<SolTkController> Controllers { get; set; }
        public DbSet<SolTkComponent> Components { get; set; }
        public DbSet<SolTkData> ControllerData { get; set; }


        public ControllerContext(DbContextOptions<ControllerContext> options) : base(options)
        {

        }
    }
}
