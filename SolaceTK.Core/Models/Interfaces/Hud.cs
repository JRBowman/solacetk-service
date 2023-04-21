using System;

namespace SolaceTK.Core.Models.Interfaces
{
    public class Hud : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Tags { get; set; }
    }
}
