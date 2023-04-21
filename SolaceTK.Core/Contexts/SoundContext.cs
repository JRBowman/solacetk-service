using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Sound;

namespace SolaceTK.Core.Contexts
{
    public class SoundContext : DbContext
    {
        public DbSet<SoundSource> SoundSources { get; set; }
        public DbSet<SoundSet> SoundSets { get; set; }
        public DbSet<SolTkData> SoundData { get; set; }
        public DbSet<SolTkCondition> SoundConditions { get; set; }

        public SoundContext(DbContextOptions<SoundContext> options) : base(options)
        {

        }
    }
}
