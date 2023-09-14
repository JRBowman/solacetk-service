using Microsoft.EntityFrameworkCore;
using SolaceTK.Models.Artifacts;
using SolaceTK.Models.Core;
using SolaceTK.Models.Story;

namespace SolaceTK.Data.Contexts
{
    public class CoreContext : DbContext
    {
        public DbSet<ResourceCollection> Collections { get; set; }

        public DbSet<Timeline> Timelines { get; set; }
        public DbSet<StoryCard> StoryCards { get; set; }

        public DbSet<GameSystem> GameSystems { get; set; }
        public DbSet<SolTkArtifact> Attachments { get; set; }


        public CoreContext(DbContextOptions<CoreContext> options) : base(options)
        {

        }
    }
}
