using Microsoft.EntityFrameworkCore;
using SolaceTK.Models.Behavior;
using SolaceTK.Models.WorkItems;
using System.Reflection.Emit;

namespace SolaceTK.Data.Contexts
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SolTkState>().HasOne(x => x.Animation).WithMany();
            //modelBuilder.Entity<SolTkSystem>().HasMany(x => x.Events).WithMany().UsingEntity(j => j.ToTable("SystemEvents"));
            //modelBuilder.Entity<SolTkSystem>().HasMany(x => x.Behaviors).WithOne().HasForeignKey(x => x.BehaviorSystemId);

            // Projects HasMany WorkItems WithOne Project
            modelBuilder.Entity<WorkProject>().HasMany(p => p.WorkItems).WithOne().HasForeignKey(x => x.WorkProjectId);
            modelBuilder.Entity<WorkProject>().HasMany(p => p.Payments).WithOne(x => x.Project);

            // Projects HasMany Comments WithOne Project
            modelBuilder.Entity<WorkProject>().HasMany(p => p.Comments).WithOne();

            modelBuilder.Entity<WorkItem>().HasMany(p => p.Artifacts).WithMany().UsingEntity(j => j.ToTable("WorkItemComments"));
            modelBuilder.Entity<WorkItem>().HasMany(p => p.Comments).WithOne();
            modelBuilder.Entity<WorkItem>().HasOne(p => p.Payment).WithMany(x => x.WorkItems);

            modelBuilder.Entity<WorkComment>().HasMany(x => x.Artifacts).WithMany().UsingEntity(j => j.ToTable("WorkCommentArtifacts"));

            modelBuilder.Entity<WorkPayment>().HasMany(x => x.WorkItems).WithOne(x => x.Payment);
            modelBuilder.Entity<WorkPayment>().HasOne(x => x.Project).WithMany();

            base.OnModelCreating(modelBuilder);
        }

    }
}
