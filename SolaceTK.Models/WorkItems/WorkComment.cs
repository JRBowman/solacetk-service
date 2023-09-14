using System;
using SolaceTK.Models;
using SolaceTK.Models.Artifacts;

namespace SolaceTK.Models.WorkItems
{
    public class WorkComment : SolTkModelBase
    {
        // Comment Info:
        public string? Comment { get; set; }
        public ICollection<SolTkArtifact>? Artifacts { get; set; } = new List<SolTkArtifact>();
    }
}
