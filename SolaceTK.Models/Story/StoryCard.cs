using SolaceTK.Models.Behavior;
using SolaceTK.Models.Events;

namespace SolaceTK.Models.Story
{
    public class StoryCard : SolTkModelBase
    {
        public bool Enabled { get; set; } = true;
        public bool Completed { get; set; } = false;

        public string? Title { get; set; }

        public int Order { get; set; }
        public long StartTime { get; set; }
        public long Duration { get; set; }
        public long EndTime { get; set; }

        // Conditions for the StoryCard to Start:
        public ICollection<SolTkCondition>? Conditions { get; set; }

        public ICollection<SolTkData>? DownstreamData { get; set; }

        // Events to Conditionally Send
        public ICollection<SolTkEvent>? Events { get; set; }

    }
}
