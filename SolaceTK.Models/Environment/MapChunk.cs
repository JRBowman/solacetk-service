namespace SolaceTK.Models.Environment
{
    public class MapChunk : IModelTK
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }

        // Chunk Properties:
        public string? ColorKey { get; set; }

        public ICollection<MapCell>? Cells { get; set; }
    }
}
