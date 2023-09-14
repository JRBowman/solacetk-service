namespace SolaceTK.Models.Behavior
{
    public class AnimationData : IModelTK
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }

        public ICollection<AnimationFrame>? Frames { get; set; } = new List<AnimationFrame>();

        public bool Enabled { get; set; }
        public bool Loop { get; set; }
        public bool Invert { get; set; }
        public bool Mirror { get; set; }
        public float Speed { get; set; }
        public int RunCount { get; set; }
    }
}
