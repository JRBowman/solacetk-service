using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Core;
using SolaceTK.Core.Models.Story;

namespace SolaceTK.Core.Contexts
{
    public class CoreContext : DbContext
    {
        public DbSet<ResourceCollection> Collections { get; set; }

        public DbSet<Timeline> Timelines { get; set; }
        public DbSet<StoryCard> StoryCards { get; set; }

        public DbSet<GameSystem> GameSystems { get; set; }
        public DbSet<SolTkAttachment> Attachments { get; set; }


        public CoreContext(DbContextOptions<CoreContext> options) : base(options)
        {

        }
    }
}
