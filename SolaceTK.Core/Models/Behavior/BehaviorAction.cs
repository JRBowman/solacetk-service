using System;
using System.Text.Json.Serialization;

namespace SolaceTK.Core.Models.Behavior
{
    [Serializable]
    public class BehaviorAction : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int StateId { get; set; }
        [JsonIgnore]
        public BehaviorState State { get; set; }

        public string StartAction { get; set; }
        public string MainAction { get; set; }
        public string EndAction { get; set; }
        
        public string Tags { get; set; }
    }
}
