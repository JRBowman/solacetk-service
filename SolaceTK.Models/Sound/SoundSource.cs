using SolaceTK.Models.Artifacts;

namespace SolaceTK.Models.Sound
{
    public class SoundSource : SolTkModelBase
    {
        
        public float Volume { get; set; }
        public float Pitch { get; set; }
        public bool PlayOnLoad { get; set; }

        public string? ClipName { get; set; }
        public string? Channel { get; set; }

        public bool IsLoop { get; set; }
        public DateTime LoopStartTime { get; set; }
        public DateTime LoopEndTime { get; set; }

        //public SolTkArtifact? Artifact { get; set; }
        public int ArtifactId { get; set; }

        public bool IsReactive { get; set; } = false;

        public ICollection<SolTkCondition>? Conditions { get; set; } = new List<SolTkCondition>();

        public ICollection<SolTkData>? SoundData { get; set; } = new List<SolTkData>();
    }
}
