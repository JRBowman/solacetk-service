namespace SolaceTK.Models.Environment
{
    public class Map : SolTkModelBase
    {
        public ICollection<MapLayer>? Layers { get; set; }
        public ICollection<MapCell>? Cells { get; set; }
        public ICollection<MapChunk>? Chunks { get; set; }

        public TileSet? TileSet { get; set; }
    }
}
