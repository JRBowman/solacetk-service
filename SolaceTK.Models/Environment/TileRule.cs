namespace SolaceTK.Models.Environment
{
    public class TileRule : IModelTK
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }

        // Rule Properties:
        public int Priority { get; set; }
        public string? ColorKey { get; set; }
        public string? DataKey { get; set; }

        // Directional TileChecks:
        // Rule Checks Tile t, where t = [vx*vm, vy*vm]: the Unit Vector * Magnitude:
        // public RuleDirection Direction { get; set; }
        public int VX { get; set; }
        public int VY { get; set; }
        public int VM { get; set; }

        public RuleCheckType? CheckType { get; set; }
    }

    public enum RuleCheckType
    {
        Disabled,
        Empty,
        Any,
        This,
        NotThis,
        Named,
        Key
    }

}
