using System;
using System.Collections.Generic;
using SolaceTK.Models;

namespace SolaceTK.Models.Environment
{
    public class MapLayer : IModelTK
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? Tags { get; set; }

        public ICollection<SolTkData>? LayerData { get; set; }

        public bool Enabled { get; set; }

        // Collision Layer:
        public string? LayerName { get; set; }

        // Drawing Layer & Order:
        public string? SortingLayer { get; set; }
        public int LayerOrder { get; set; }

        public bool IsCollidable { get; set; }
        public bool IsBreakable { get; set; }
    }
}
