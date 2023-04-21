using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Sound
{
    public class SoundSet : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<SoundSource> Sources { get; set; }
        
        public string Tags { get; set; }
    }
}
