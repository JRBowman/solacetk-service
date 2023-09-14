using SolaceTK.Models.Controllers;
using SolaceTK.Models.Identity;
using SolaceTK.Models.Sound;

namespace SolaceTK.Models
{
    public class SolTkController : IControllerModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }

        public string? PixelKeyColor { get; set; }

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

        public ICollection<SolTkData>? ControllerData { get; set; }
        public ICollection<SolTkComponent>? Components { get; set; }
        public SoundSet? SoundSet { get; set; }

        public MovableControllerType Type { get; set; }
        public SolTkBodyType BodyType { get; set; }
        public SolTkUser? User { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }

    public enum MovableControllerType
    {
        NavMeshAgent,
        Behavior,
        Dynamic
    }
}
