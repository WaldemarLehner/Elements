using System.Collections.Generic;
using System.Windows.Forms;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Interfaces
{
    internal interface IModel
    {
        (float top, float bottom, float left, float right) CurrentSceneBounds { get; }

        IEnumerable<IUiRenderable> UiRenderables { get; }

        IEnumerable<IRenderable> Renderables { get; }

        void Update(float dTime);

        bool CreatePlayerOnce(IInputController controller);

        bool SpawnHeal(float positionX, float positionY);

        bool SpawnWährung(float positionX, float positionY);

        bool CreateRoundEndInteractables();

        bool CreateEnemy();

        void DestroyObject(IPlayer player, IEntity entity, INonPlayerCharacter npc);

        void CreateProjectile(int projectileCreationCount, Vector2 position, Vector2 direction, int bulletTTL, float bulletSize, IColliderManager colliderManager);
    }
}
