using Microsoft.EntityFrameworkCore;
using SolaceTK.Models;
using SolaceTK.Models.Environment;

namespace SolaceTK.Data.Contexts
{
    public class EnvironmentContext : DbContext
    {
        public DbSet<Map> Maps { get; set; }
        public DbSet<MapCell> Cells { get; set; }
        public DbSet<MapChunk> Chunks { get; set; }
        public DbSet<MapLayer> Layers { get; set; }

        public DbSet<SolTkData> EnvironmentData { get; set; }

        public DbSet<TileSet> TileSets { get; set; }
        public DbSet<Tile> Tiles { get; set; }
        public DbSet<TileRule> TileRules { get; set; }



        public EnvironmentContext(DbContextOptions<EnvironmentContext> options) : base(options)
        {

        }
    }
}
