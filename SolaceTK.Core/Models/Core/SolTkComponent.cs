using System.Collections.Generic;

namespace SolaceTK.Core.Models.Core
{
    public class SolTkComponent : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Tags { get; set; }

        public float PositionX { get; set; } = 0f;
        public float PositionY { get; set; } = 0f;
        public float PositionZ { get; set; } = 0f;

        public float RotationX { get; set; } = 0f;
        public float RotationY { get; set; } = 0f;
        public float RotationZ { get; set; } = 0f;

        public float ScaleX { get; set; } = 1f;
        public float ScaleY { get; set; } = 1f;
        public float ScaleZ { get; set; } = 1f;

        public ICollection<SolTkData> ComponentData { get; set; }

        public SolTkComponentTypes ComponentType;

        public int MovableControllerId { get; set; }
        public int BehaviorAnimationId { get; set; }

    }

    public enum SolTkComponentTypes
    {
        SpriteRenderer,
        RayCastPoint,
        SoundEmitter,
        TargetPoint,
        Controller,
        AudioEffect,
        VisualEffect,
        ParticleSystem,
        GeneralObject,
        EngineAnimator,
        Collidable
    }
}
