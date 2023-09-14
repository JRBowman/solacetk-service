namespace SolaceTK.Models.Sound
{
    public class SoundSet : SolTkModelBase
    {
        public ICollection<SoundSource>? Sources { get; set; }
    }
}
