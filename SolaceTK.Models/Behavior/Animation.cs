namespace SolaceTK.Models.Behavior
{
    public class Animation : SolTkModelBase
    {
        //public BehaviorAnimationData StartFrameData { get; set; }
        public AnimationData? ActFrameData { get; set; }
        public ICollection<SolTkComponent>? Components { get; set; } = new List<SolTkComponent>();
        //public BehaviorAnimationData EndFrameData { get; set; }
    }

}
