using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Behavior
{
    [Serializable]
    public class BehaviorCondition : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public ICollection<SolTkCondition> Conditions { get; set; }
        public string Tags { get; set; }
    }
}
