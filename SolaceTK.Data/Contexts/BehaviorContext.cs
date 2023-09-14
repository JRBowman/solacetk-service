using Microsoft.EntityFrameworkCore;
using SolaceTK.Models;
using SolaceTK.Models.Behavior;
using SolaceTK.Models.Events;

namespace SolaceTK.Data.Contexts
{
    public class BehaviorContext : DbContext
    {
        public DbSet<Animation> Animations { get; set; }
        public DbSet<SolTkState> States { get; set; }
        public DbSet<SolTkSystem> Systems { get; set; }

        public DbSet<SolTkData> AttributeData { get; set; }
        public DbSet<SolTkCondition> ConditionsData { get; set; }

        public DbSet<SolTkEvent> Events { get; set; }
        public DbSet<SolTkMessage> Messages { get; set; }

        public DbSet<AnimationData> AnimationData { get; set; }
        public DbSet<AnimationFrame> Frames { get; set; }
        public DbSet<SolTkComponent> Components { get; set; }


        public BehaviorContext(DbContextOptions<BehaviorContext> options) : base(options)
        {


        }

        // Model Relationships:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SolTkState>().HasOne(x => x.Animation).WithMany();

            modelBuilder.Entity<SolTkState>().HasMany(x => x.Events).WithMany().UsingEntity(j => j.ToTable("StateEvents"));
            modelBuilder.Entity<SolTkSystem>().HasMany(x => x.Events).WithMany().UsingEntity(j => j.ToTable("SystemEvents"));
            modelBuilder.Entity<SolTkSystem>().HasMany(x => x.Behaviors).WithOne().HasForeignKey(x => x.BehaviorSystemId);
            modelBuilder.Entity<SolTkState>().HasMany(x => x.NextStates).WithMany().UsingEntity(j => j.ToTable("StateBranches"));

            modelBuilder.Entity<Animation>().HasMany(x => x.Components).WithOne().HasForeignKey(x => x.BehaviorAnimationId);
            modelBuilder.Entity<AnimationFrame>().HasMany(x => x.DownstreamData).WithOne();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase("onbow-behavior");
    }
}
