using SolaceTK.Core.Models.Behavior;
using SolaceTK.Core.Models.Controllers;
using SolaceTK.Core.Models.Environment;
using SolaceTK.Core.Models.Interfaces;
using SolaceTK.Core.Models.Sound;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.Core
{
    public class ResourceCollection : IModelTK
    {
        #region Controllers:

        public ICollection<MovableController> Movables { get; set; }
        public ICollection<ImmovableController> Immovables { get; set; }
        public ICollection<NavigationController> Navigations { get; set; }
        public ICollection<TransportController> Transports { get; set; }
        public ICollection<CharacterController> Characters { get; set; }
        public ICollection<EnemyController> Enemies { get; set; }
        public ICollection<StaticObjectController> Objects { get; set; }
        public string Tags { get; set; }

        #endregion

        #region Behaviors:

        public ICollection<BehaviorSystem> Behaviors { get; set; }

        #endregion


        #region Environments:

        public ICollection<TileSet> TileSets { get; set; }

        #endregion

        #region Interfaces:

        public ICollection<Hud> Huds { get; set; }
        public ICollection<HudDialog> Dialogs { get; set; }
        public ICollection<HudMenu> Menus { get; set; }

        #endregion

        #region Sounds:

        public ICollection<SoundSet> SoundSets { get; set; }
        public ICollection<SoundSource> SoundSources { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        

        #endregion
    }
}
