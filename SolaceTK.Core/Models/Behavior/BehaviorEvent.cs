using System;
using System.Collections.Generic;
using SolaceTK.Core.Models.Controllers;
using SolaceTK.Core.Models.Environment;

namespace SolaceTK.Core.Models.Behavior
{
    [Serializable]
    public class BehaviorEvent : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Tags { get; set; }

        public bool IsPhysicsEvent { get; set; }

        public float DebounceTime { get; set; }

        public ICollection<SolTkCondition> Conditions { get; set; }
        public ICollection<SolTkData> DownstreamData { get; set; }

        public ICollection<BehaviorMessage> Messages { get; set; }

        // Reverse Lookups:
        // public ICollection<BehaviorState> BehaviorStates { get; set; }
        // public ICollection<BehaviorSystem> BehaviorSystems { get; set; }
        // public ICollection<MovableController> MovableControllers { get; set; }
        // public ICollection<MapCell> MapCells { get; set; }
    }
}
