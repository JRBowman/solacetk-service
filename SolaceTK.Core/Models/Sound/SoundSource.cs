using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Sound
{
    [Serializable]
    public class SoundSource : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Tags { get; set; }

        public bool IsLoop { get; set; }
        public float Volume { get; set; }
        public float Pitch { get; set; }
        public bool PlayOnLoad { get; set; }
        public string ClipName { get; set; }
        //public byte[] Clip { get; set; }
        public DateTime LoopStartTime { get; set; }
        public DateTime LoopEndTime { get; set; }

        public ICollection<SolTkData> SoundData { get; set; }
    }
}
