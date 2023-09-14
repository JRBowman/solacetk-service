namespace SolaceTK.Models.Environment
{
    public class Tile : SolTkModelBase
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }

        // Tile Properties:
        public string? ColorKey { get; set; }
        public TileType Type { get; set; }
        public TileTransformMode Mode { get; set; }

        // Sizing of Tile on the Sprite Sheet:
        public int LX { get; set; }
        public int LY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // Used to spawn Objects at the Tile Location:
        public string? ObjectKey { get; set; }

        public ICollection<SolTkData>? Data { get; set; }

        // Directional Rules:
        public ICollection<TileRule>? Rules { get; set; }
    }

    public enum TileType
    {
        Sprite,
        Object,
        Both
    }

    public enum TileTransformMode
    {
        None,
        Rotate,
        MirrorX,
        MirrorY,
        MirrorXY
    }
}
