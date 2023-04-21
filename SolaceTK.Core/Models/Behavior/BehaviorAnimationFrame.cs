using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Behavior
{
    [Serializable]
    public class BehaviorAnimationFrame : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        public int Order { get; set; }
        public float Speed { get; set; } = 1f;
        public float Duration { get; set; }
        public string FrameData { get; set; }
        public ICollection<SolTkData> DownstreamData { get; set; }

    }
}
