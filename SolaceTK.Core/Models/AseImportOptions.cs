using System.Collections.Generic;

namespace SolaceTK.Core.Models
{
    public class AseImportOptions
    {
        public bool SplitTags { get; set; } = false; // --split-tags
        public bool SplitLayers { get; set; } = false; // --split-layers
        public AseImportMode AseImportMode { get; set; } = AseImportMode.SpriteSheet; // .gif / .png -or- --sheet {SheetName}.png
        public AseSheetType SheetType { get; set; } = AseSheetType.Rows; // --sheet-type {AseSheetType}
        public string Tag { get; set; } = ""; // --tag "{Tag}"

        // --frame-range [FromRange, ToRange];
        public int FromFrame { get; set; } = 0;
        public int ToFrame { get; set; } = 0;

        public static List<string> AseSheetTypeStings = new() { "horizontal", "vertical", "rows", "columns", "packed" };

    }

    public enum AseImportMode
    {
        Gif,
        SpriteSheet,
        Png
    }

    public enum AseSheetType
    {
        Horizontal,
        Vertical,
        Rows,
        Columns,
        Packed
    }
}
