namespace SolaceTK.Models.Behavior
{
    public class AnimationFrame : IModelTK
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }

        public int Order { get; set; }
        public float Speed { get; set; } = 1f;
        public float Duration { get; set; }
        public string? FrameData { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ICollection<SolTkData>? DownstreamData { get; set; } = new List<SolTkData>();

    }
}
