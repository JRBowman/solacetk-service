using SolaceTK.Core.Models.Behavior;
using SolaceTK.Core.Models.Sound;
using System;
using System.Collections.Generic;
using SolaceTK.Core.Models.Core;

namespace SolaceTK.Core.Models.Controllers
{
    public class MovableController : IControllerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string PixelKeyColor { get; set; }
        
        public float WorldPositionX { get; set; }
        public float WorldPositionY { get; set; }
        public float WorldPositionZ { get; set; }

        public int MapPositionX { get; set; }
        public int MapPositionY { get; set; }
        
        public CollisionDetectionType CollisionType { get; set; }

        public bool UseFriction { get; set; }
        public bool AffectedByGravity { get; set; }
        public bool CanMove { get; set; }
        public float Mass { get; set; }

        public int BehaviorSystemId { get; set; }
        //public BehaviorSystem BehaviorSystem { get; set; }

        public ICollection<SolTkData> ControllerData { get; set; }
        public ICollection<SolTkComponent> Components { get; set; }
        public SoundSet SoundSet { get; set; }

        public MovableControllerType Type { get; set; }
        

        public string Tags { get; set; }
    }

    public enum MovableControllerType
    {
        NavMeshAgent,
        Behavior,
        Dynamic
    }
}
