using SolaceTK.Core.Models.Story;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Core
{
    public class Timeline : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        public ICollection<StoryCard> StoryCards { get; set; }

        public long Length { get; set; }
    }
}
