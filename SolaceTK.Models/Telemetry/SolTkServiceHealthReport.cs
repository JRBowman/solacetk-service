namespace SolaceTK.Models.Telemetry
{
    internal class SolTkServiceHealthReport
    {
        public List<string> ServiceStatus { get; set; } = new();
        public List<string> DataStatus { get; set; } = new();
        public List<string> AsepriteStatus { get; set; } = new();
    }
}
