namespace SolaceTK.Models.Events
{
    /// <summary>
    /// An Event defines Data Changes and Messages that get executed under certain Conditions.
    /// </summary>
    public class SolTkEvent : SolTkModelBase
    {
        public bool IsPhysicsEvent { get; set; }
        public float DebounceTime { get; set; }

        public ICollection<SolTkCondition> Conditions { get; set; } = new List<SolTkCondition>();
        public ICollection<SolTkData> DownstreamData { get; set; } = new List<SolTkData>();

        public ICollection<SolTkMessage> Messages { get; set; } = new List<SolTkMessage>();
    }
}
