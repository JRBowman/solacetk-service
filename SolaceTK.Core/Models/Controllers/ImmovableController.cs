using System;
using System.Collections.Generic;
using System.Drawing;

namespace SolaceTK.Core.Models.Controllers
{
    public class ImmovableController : IControllerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string PixelKeyColor { get; set; }
        //public ICollection<EnvironmentData> ControllerData { get; set; }
        public float WorldPositionX { get; set; }
        public float WorldPositionY { get; set; }
        public float WorldPositionZ { get; set; }
        public int MapPositionX { get; set; }
        public int MapPositionY { get; set; }
        public CollisionDetectionType CollisionType { get; set; }
        public bool IsHit { get; set; }
        
        public string Tags { get; set; }
    }
}
