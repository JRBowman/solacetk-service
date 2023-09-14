namespace SolaceTK.Models.Artifacts
{
    public interface ISolTkArtifact : IModelTK
    {
        public string? ArtifactName { get; set; }
        public string? ArtifactUrl { get; set; }
        public string? ArtifactDirectory { get; set; }
    }
}
