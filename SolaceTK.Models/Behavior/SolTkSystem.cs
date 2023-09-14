using SolaceTK.Models.Events;

namespace SolaceTK.Models.Behavior
{
    [Serializable]
    public class SolTkSystem : SolTkModelBase
    {
        public ICollection<SolTkData>? VarData { get; set; } = new List<SolTkData>();
        public ICollection<SolTkState>? Behaviors { get; set; } = new List<SolTkState>();
        public ICollection<SolTkEvent>? Events { get; set; } = new List<SolTkEvent>();
    }
}
