using Microsoft.EntityFrameworkCore;
using SolaceTK.Models;
using SolaceTK.Models.Artifacts;
using SolaceTK.Models.Sound;

namespace SolaceTK.Data.Contexts
{
    public class SoundContext : DbContext
    {
        public DbSet<SoundSource> SoundSources { get; set; }
        public DbSet<SoundSet> SoundSets { get; set; }
        public DbSet<SolTkData> SoundData { get; set; }
        public DbSet<SolTkCondition> Conditions { get; set; }

        public DbSet<SolTkArtifact> Artifacts { get; set; }

        public SoundContext(DbContextOptions<SoundContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setup SoundSets:
            modelBuilder.Entity<SoundSet>().HasMany(x => x.Sources).WithMany().UsingEntity(j => j.ToTable("SoundSetSources"));

            //modelBuilder.Entity<SoundSource>().HasOne(x => x.Artifact).WithMany();
            //modelBuilder.Entity<SoundSource>().HasOne<SolTkArtifact>().WithMany().HasForeignKey(x => x.ArtifactId);
            modelBuilder.Entity<SoundSource>().HasMany(x => x.Conditions).WithMany().UsingEntity(j => j.ToTable("SoundSourceConditions"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
