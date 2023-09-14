using SolaceTK.Models.Events;

namespace SolaceTK.Models.Behavior
{
    [Serializable]
    public class SolTkState : SolTkModelBase
    {
        public string? StateType { get; set; }
        public int RunCount { get; set; }
        public bool NoOp { get; set; } = false;
        public bool Enabled { get; set; } = true;

        public Animation? Animation { get; set; }
        public ICollection<SolTkEvent>? Events { get; set; } = new List<SolTkEvent>();
        public ICollection<SolTkCondition>? Conditions { get; set; } = new List<SolTkCondition>();
        public ICollection<SolTkData>? StartData { get; set; } = new List<SolTkData>();
        public ICollection<SolTkData>? ActData { get; set; } = new List<SolTkData>();
        public ICollection<SolTkData>? EndData { get; set; } = new List<SolTkData>();
        public ICollection<SolTkState>? NextStates { get; set; } = new List<SolTkState>();

        // Associated System Owner:
        public int BehaviorSystemId { get; set; }
        // Associated State Owner:
        public int ParentId { get; set; }

        public float StartDelay { get; set; }
        public float EndDelay { get; set; }
        public bool Interruptable { get; set; }
    }
}
