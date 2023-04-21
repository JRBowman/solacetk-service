namespace SolaceTK.Core.Models
{
    public class SolTkAttachment : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileLocation { get; set; }
    }
}
