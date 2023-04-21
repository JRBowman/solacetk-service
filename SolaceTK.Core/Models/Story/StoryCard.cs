using SolaceTK.Core.Models.Behavior;
using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Story
{
    public class StoryCard : IModelTK
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        public bool Enabled { get; set; } = true;
        public bool Completed { get; set; } = false;

        public string Title { get; set; }

        public int Order { get; set; }
        public long StartTime { get; set; }
        public long Duration { get; set; }
        public long EndTime { get; set; }

        // Conditions for the StoryCard to Start:
        public ICollection<SolTkCondition> Conditions { get; set; }

        public ICollection<SolTkData> DownstreamData { get; set; }

        // Events to Conditionally Send
        public ICollection<BehaviorEvent> Events { get; set; }

    }
}
