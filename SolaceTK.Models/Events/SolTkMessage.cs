namespace SolaceTK.Models.Events
{
    /// <summary>
    /// A Message is a collection of Data Changes that should be applied to a Target. Targets can include Hubs, Named Targets, etc.
    /// </summary>
    public class SolTkMessage : SolTkModelBase
    {
        public ICollection<SolTkData> Data { get; set; } = new List<SolTkData>();

        public string? TargetName { get; set; }
        public MessageTarget TargetType { get; set; }

    }

    public enum MessageTarget
    {
        NamedTarget,
        InstanceTarget,
        CollisionTarget,
        FocusTarget,
        MessageQueue
    }
}
