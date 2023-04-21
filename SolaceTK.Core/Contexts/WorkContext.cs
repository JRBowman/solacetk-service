using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Models.WorkItems;

namespace SolaceTK.Core.Contexts
{
    public class WorkContext : DbContext
    {

        public DbSet<WorkProject> Projects { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<WorkComment> Comments { get; set; }
        public DbSet<WorkPayment> Payments { get; set; }
        public DbSet<WorkArtifact> Artifacts { get; set; }


        public WorkContext(DbContextOptions<WorkContext> options) : base(options)
        {

        }

    }
}
