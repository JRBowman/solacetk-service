namespace SolaceTK.Core.Models.Files
{
    public class AseFile
    {
        public string Name { get; set; }
        public string Directory { get; set; }


        public string AseName => Name + ".ase";
        public string SheetName => Name + ".png";
        public string GifName => Name + ".gif";
    }
}
