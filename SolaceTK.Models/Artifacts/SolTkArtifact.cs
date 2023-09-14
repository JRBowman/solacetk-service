namespace SolaceTK.Models.Artifacts
{
    public class SolTkArtifact : SolTkModelBase, ISolTkArtifact
    {
        public string? ArtifactName { get; set; }
        public string? ArtifactExtension { get; set; }
        public string? ArtifactUrl { get; set; }
        public string? ArtifactDirectory { get; set; }
        public string? CollectionRoot { get; set; }
    }
}
