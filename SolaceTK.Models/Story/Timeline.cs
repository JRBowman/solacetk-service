namespace SolaceTK.Models.Story
{
    public class Timeline : SolTkModelBase
    {
        public ICollection<StoryCard>? StoryCards { get; set; }

        public long Length { get; set; }
    }
}
