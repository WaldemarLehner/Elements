using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.World;
using ComputergrafikSpiel.Model.World.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Scene
{
    public interface IScene : IUpdateable
    {
        IRenderableBackground Background { get; }

        IColliderManager ColliderManager { get; }

        IEnumerable<IEntity> Entities { get; }

        IScene TopScene { get; }

        IScene RightScene { get; }

        IScene LeftScene { get; }

        IScene BottomScene { get; }

        IEnumerable<INonPlayerCharacter> NPCs { get; }

        List<(Color4 color, Vector2[] verts)> IndependentDebugData { get; }

        IWorldScene World { get; }

        IModel Model { get; }

        void SetAsActive();

        void Disable();
    }
}
