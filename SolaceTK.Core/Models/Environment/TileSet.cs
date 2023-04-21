using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Environment
{
    public class TileSet : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Tags { get; set; }

        public ICollection<Tile> Tiles { get; set; }
    }
}
