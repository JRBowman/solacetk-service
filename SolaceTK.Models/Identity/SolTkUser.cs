namespace SolaceTK.Models.Identity
{
    public class SolTkUser : ISolTkUser
    {
        // ModelTk Details:
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }

        // SolTkUser Details:
        public string? Username { get; set; }
        public SolTkUserType Type { get; set; }
    }
}
