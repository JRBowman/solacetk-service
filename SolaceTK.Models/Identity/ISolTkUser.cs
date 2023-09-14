namespace SolaceTK.Models.Identity
{
    public interface ISolTkUser : IModelTK
    {
        public string? Username { get; set; }
        public SolTkUserType Type { get; set; }
    }

    public enum SolTkUserType
    {
        Viewer,
        Commenter,
        Tester,
        Advisor,
        Contributor,
        Maintainer,
        Administrator
    }
}
