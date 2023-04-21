using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using SolaceTK.Core.Models.Core;

namespace SolaceTK.Core.Models.Behavior
{
    [Serializable]
    public class BehaviorAnimation : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        //public BehaviorAnimationData StartFrameData { get; set; }
        public BehaviorAnimationData ActFrameData { get; set; }
        public ICollection<SolTkComponent> Components { get; set; }
        //public BehaviorAnimationData EndFrameData { get; set; }
    }

    [Serializable]
    public class BehaviorAnimationData : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        public bool Enabled { get; set; }

        public ICollection<BehaviorAnimationFrame> Frames { get; set; }

        public bool Loop { get; set; }
        public bool Invert { get; set; }
        public bool Mirror { get; set; }
        public float Speed { get; set; }

        public float RunCount { get; set; }

    }

}
