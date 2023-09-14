namespace SolaceTK.Models.Events
{
    /// <summary>
    /// A Hub defines a Message / Event Queue that should be used and maintained during runtime.
    /// </summary>
    public class SolTkHub : SolTkModelBase
    {
        // Hub Specific Data:
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? PriorityRegex { get; set; }
    }
}
