using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Models.Behavior;
using SolaceTK.Core.Models.Core;
using SolaceTK.Core.Models;

namespace SolaceTK.Core.Contexts
{
    public class BehaviorContext : DbContext
    {
        public DbSet<BehaviorAnimation> Animations { get; set; }
        public DbSet<BehaviorAction> Actions { get; set; }
        public DbSet<BehaviorState> States { get; set; }
        public DbSet<BehaviorSystem> Systems { get; set; }
        public DbSet<BehaviorCondition> Conditions { get; set; }

        public DbSet<SolTkData> AttributeData { get; set; }
        public DbSet<SolTkCondition> ConditionsData { get; set; }

        public DbSet<BehaviorEvent> Events { get; set; }
        public DbSet<BehaviorMessage> Messages { get; set; }

        public DbSet<BehaviorAnimationData> AnimationData { get; set; }
        public DbSet<BehaviorAnimationFrame> Frames { get; set; }
        public DbSet<SolTkComponent> Components { get; set; }


        public BehaviorContext(DbContextOptions<BehaviorContext> options) : base(options)
        {

            
        }

        // Model Relationships:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BehaviorState>().HasOne<BehaviorAnimation>(x => x.Animation).WithMany();

            //modelBuilder.Entity<BehaviorState>().HasOne<BehaviorState>(x => x.Parent).WithMany(x => x.NextStates).HasForeignKey(x => x.ParentId);

            modelBuilder.Entity<BehaviorState>().HasMany<BehaviorEvent>(x => x.Events).WithMany().UsingEntity(j => j.ToTable("StateEvents"));
            modelBuilder.Entity<BehaviorSystem>().HasMany<BehaviorEvent>(x => x.Events).WithMany().UsingEntity(j => j.ToTable("SystemEvents"));
            modelBuilder.Entity<BehaviorSystem>().HasMany<BehaviorState>(x => x.Behaviors).WithOne().HasForeignKey(x => x.BehaviorSystemId);
            modelBuilder.Entity<BehaviorState>().HasMany<BehaviorState>(x => x.NextStates).WithMany().UsingEntity(j => j.ToTable("StateBranches"));
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase("onbow-behavior");
    }
}
