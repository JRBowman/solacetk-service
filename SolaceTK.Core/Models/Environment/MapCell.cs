﻿using SolaceTK.Core.Models.Behavior;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Environment
{
    public class MapCell : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        // Cell Properties:
        public int X { get; set; }
        public int Y { get; set; }
        public string ColorKey { get; set; }
        public bool Enabled { get; set; }


        public ICollection<SolTkData> EnterData { get; set; }

        public ICollection<SolTkData> ActiveData { get; set; }
        public ICollection<BehaviorEvent> BehaviorEvents { get; set; }

        public ICollection<SolTkData> ExitData { get; set; }
    }
}
