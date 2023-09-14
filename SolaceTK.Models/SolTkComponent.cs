using SolaceTK.Models.Identity;

namespace SolaceTK.Models
{
    public class SolTkComponent : SolTkModelBase
    {
        public float PositionX { get; set; } = 0f;
        public float PositionY { get; set; } = 0f;
        public float PositionZ { get; set; } = 0f;

        public float RotationX { get; set; } = 0f;
        public float RotationY { get; set; } = 0f;
        public float RotationZ { get; set; } = 0f;

        public float ScaleX { get; set; } = 1f;
        public float ScaleY { get; set; } = 1f;
        public float ScaleZ { get; set; } = 1f;

        public string ColorKey { get; set; } = "";

        public bool Enabled { get; set; } = false;

        public ICollection<SolTkData>? ComponentData { get; set; } = new List<SolTkData>();

        public SolTkComponentTypes ComponentType { get; set; }

        public int ControllerId { get; set; }
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
