using System.Collections.Generic;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.World;

namespace ComputergrafikSpiel.Model.Scene
{
    public interface IScene : IUpdateable
    {
        IWorldTile WorldTile { get; }

        IColliderManager ColliderManager { get; }

        ICollection<IEntity> Entities { get; }

        IScene TopScene { get; }

        IScene RightScene { get; }

        IScene LeftScene { get; }

        IScene BottomScene { get; }

        void SwitchScene(IScene scene);
    }
}
