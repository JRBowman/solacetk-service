using System;
using System.Collections.Generic;
using System.Drawing;

namespace SolaceTK.Core.Models.Controllers
{
    public interface IControllerModel : IModelTK
    {
        public string PixelKeyColor { get; set; }
        //public ICollection<EnvironmentData> ControllerData { get; set; }
        public float WorldPositionX { get; set; }
        public float WorldPositionY { get; set; }
        public float WorldPositionZ { get; set; }
        
        public int MapPositionX { get; set; }
        public int MapPositionY { get; set; }

        // CollidableBody:
        public CollisionDetectionType CollisionType { get; set; }
    }

    public enum CollisionDetectionType
    {
        RayCast,
        BoxCast,
        CircleCast,
        CapsuleCast
    }
}
