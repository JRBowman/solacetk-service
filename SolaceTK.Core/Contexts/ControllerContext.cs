using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Controllers;
using SolaceTK.Core.Models.Core;

namespace SolaceTK.Core.Contexts
{
    public class ControllerContext : DbContext
    {
        public DbSet<MovableController> Controllers { get; set; }
        public DbSet<SolTkComponent> Components { get; set; }
        public DbSet<SolTkData> ControllerData { get; set; }


        public ControllerContext(DbContextOptions<ControllerContext> options) : base(options)
        {

        }
    }
}
