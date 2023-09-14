using SolaceTK.Models.Behavior;
using SolaceTK.Models.Environment;
using SolaceTK.Models.Interfaces;
using SolaceTK.Models.Sound;

namespace SolaceTK.Models.Core
{
    public class ResourceCollection : SolTkModelBase
    {

        #region Controllers:

        public ICollection<SolTkController>? Controllers { get; set; }
        public ICollection<SolTkController>? Characters { get; set; }
        public ICollection<SolTkController>? NPCs { get; set; }
        public ICollection<SolTkController>? Objects { get; set; }
        public ICollection<SolTkController>? Navigation { get; set; }
        public ICollection<SolTkController>? Transports { get; set; }


        #endregion

        #region Behaviors:

        public ICollection<SolTkSystem>? Behaviors { get; set; }

        #endregion

        #region Environments:

        public ICollection<Map>? Maps { get; set; }
        public ICollection<TileSet>? TileSets { get; set; }

        #endregion

        #region Interfaces:

        public ICollection<Hud>? Huds { get; set; }
        public ICollection<HudDialog>? Dialogs { get; set; }
        public ICollection<HudMenu>? Menus { get; set; }

        #endregion

        #region Sounds:

        public ICollection<SoundSet>? SoundSets { get; set; }
        public ICollection<SoundSource>? SoundSources { get; set; }

        #endregion
    }
}
