using System.Collections.Generic;
using System.Windows.Forms;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;

namespace ComputergrafikSpiel.Model.Interfaces
{
    internal interface IModel
    {
        (float top, float bottom, float left, float right) CurrentSceneBounds { get; }

        IEnumerable<IUiRenderable> UiRenderables { get; }

        IEnumerable<IRenderable> Renderables { get; }

        void Update(float dTime);

        bool CreatePlayerOnce(IInputController controller);

        bool SpawnHeal(float playerPositionX, float playerPositionY);

        bool SpawnWährung(float playerPositionX, float playerPositionY);

        bool CreateRoundEndInteractables();

        bool CreateEnemy();

        void DestroyObject(IPlayer player, IEntity entity, INonPlayerCharacter npc);
    }
}
