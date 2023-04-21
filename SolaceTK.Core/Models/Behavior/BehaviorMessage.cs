using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Behavior
{
    [Serializable]
    public class BehaviorMessage : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Tags { get; set; }

        public ICollection<SolTkData> Data { get; set; }

        public string TargetName { get; set; }
        public MessageTarget TargetType { get; set; }

    }

    public enum MessageTarget
    {
        NamedTarget,
        InstanceTarget,
        CollisionTarget,
        FocusTarget,
        MessageQueue
    }
}
