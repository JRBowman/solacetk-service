using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Behavior
{
    [Serializable]
    public class BehaviorState : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Tags { get; set; }


        public string StateType { get; set; }
        public int RunCount { get; set; }
        public bool NoOp { get; set; } = false;
        public bool Enabled { get; set; } = true;

        public BehaviorAnimation Animation { get; set; }
        public ICollection<BehaviorEvent> Events { get; set; }
        public ICollection<SolTkCondition> Conditions { get; set; }
        public ICollection<SolTkData> StartData { get; set; }
        public ICollection<SolTkData> ActData { get; set; }
        public ICollection<SolTkData> EndData { get; set; }
        public ICollection<BehaviorState> NextStates { get; set; }

        // Parent (Many to Many?)
        //public int BehaviorStateId { get; set; }
        //public ICollection<BehaviorState> BehaviorStates { get; set; }

        // Associated System Owner:
        public int BehaviorSystemId { get; set; }
        // Associated State Owner:
        public int ParentId { get; set; }

        public string StartDelay { get; set; }
        public string EndDelay { get; set; }
        public bool Interruptable { get; set; }
    }
}
