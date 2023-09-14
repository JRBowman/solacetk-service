namespace SolaceTK.Models.Events
{
    /// <summary>
    /// HubTopic defines a Message Queue Topic that should be upserted and maintained by the Event Hub.
    /// </summary>
    public class SolTkHubTopic : IModelTK
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }

        // Topic Data:
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? PriorityRegex { get; set; }
    }
}
