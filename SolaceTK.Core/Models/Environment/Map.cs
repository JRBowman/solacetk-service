using System.Collections.Generic;

namespace SolaceTK.Core.Models.Environment
{
    public class Map : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        

        public string Tags { get; set; }

        public ICollection<MapLayer> Layers { get; set; }
        public ICollection<MapCell> Cells { get; set; }
        public ICollection<MapChunk> Chunks { get; set; }


        public TileSet TileSet { get; set; }
    }
}
