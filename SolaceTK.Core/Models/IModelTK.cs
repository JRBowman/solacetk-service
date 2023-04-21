using System;

namespace SolaceTK.Core.Models
{
    public interface IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; } // Setup like "#player #movable"
    }
}
