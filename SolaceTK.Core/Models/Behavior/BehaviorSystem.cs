using SolaceTK.Core.Controllers;
using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Behavior
{
    [Serializable]
    public class BehaviorSystem : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public BehaviorState StartState { get; set; }

        public BehaviorType BehaviorType { get; set; }

        public ICollection<SolTkData> VarData { get; set; }
        public ICollection<BehaviorState> Behaviors { get; set; }

        //public ICollection<BehaviorBranch> Branches { get; set; }
        public ICollection<BehaviorEvent> Events { get; set; }
        
        public string Tags { get; set; }
    }

    public enum BehaviorType
    {
        Neutral = 0,
        Friendly = 1,
        Hostile = 2,
        Timid = 3,
        Incapacitated = 4,
        Unconcious = 5,
        Imperfect = 6,
        Complex = 7
    }
}
