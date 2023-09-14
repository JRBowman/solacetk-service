using SolaceTK.Models.Artifacts;

namespace SolaceTK.Models.WorkItems
{
    public class WorkItem : SolTkModelBase
    {
        // Work Item Details:
        public float HoursEstimate { get; set; }
        public float HoursActual { get; set; }

        public WorkPayment? Payment { get; set; }
        public bool IsPaid { get; set; }

        public int WorkProjectId { get; set; }

        public ICollection<WorkComment>? Comments { get; set; } = new List<WorkComment>();
        public ICollection<SolTkArtifact>? Artifacts { get; set; } = new List<SolTkArtifact>();
    }
}
