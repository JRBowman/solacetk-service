using System;
using System.Collections.Generic;
using System.Drawing;
using SolaceTK.Models;

namespace SolaceTK.Models.Controllers
{
    public interface IControllerModel : IModelTK, IAuditableTK
    {
        public string? PixelKeyColor { get; set; }
        //public ICollection<EnvironmentData> ControllerData { get; set; }
        public float WorldPositionX { get; set; }
        public float WorldPositionY { get; set; }
        public float WorldPositionZ { get; set; }

        public int MapPositionX { get; set; }
        public int MapPositionY { get; set; }

        // CollidableBody:
        public CollisionDetectionType CollisionType { get; set; }

        public SolTkBodyType BodyType { get; set; }
    }

    public enum CollisionDetectionType
    {
        RayCast,
        BoxCast,
        CircleCast,
        CapsuleCast
    }

    public enum SolTkBodyType
    {
        Static,
        Kinematic,
        Dynamic
    }
}
