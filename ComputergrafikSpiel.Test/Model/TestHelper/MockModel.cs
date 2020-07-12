using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Overlay;
using OpenTK;
using System;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Test.Model.TestHelper
{
    public class MockModel : IModel
    {
        public (float top, float bottom, float left, float right) CurrentSceneBounds => (100, 0, 0, 100);

        public List<IGUIElement[]> UiRenderableList { get; set; } = new List<IGUIElement[]>();

        public IEnumerable<IGUIElement[]> UiRenderables => UiRenderableList;

        public List<IRenderable> RenderableList { get; set; } = new List<IRenderable>();

        public IEnumerable<IRenderable> Renderables => RenderableList;

        public int Level => throw new NotImplementedException();

        public bool CreatePlayerOnce(IInputController controller)
        {
            throw new NotImplementedException();
        }

        public bool CreateEnemy()
        {
            throw new NotImplementedException();
        }

        public bool CreateSoloInteractable()
        {
            throw new NotImplementedException();
        }

        public void DestroyObject(IPlayer player, IEntity entity, INonPlayerCharacter npc)
        {
            throw new NotImplementedException();
        }

        public void CreateTestWeapon()
        {
            throw new NotImplementedException();
        }

        public void Update(float dTime)
        {
            throw new NotImplementedException();
        }

        public bool CreateRoundEndInteractables()
        {
            throw new NotImplementedException();
        }

        public bool SpawnHeal(float positionX, float positionY)
        {
            throw new NotImplementedException();
        }

        public bool SpawnWährung(float positionX, float positionY)
        {
            throw new NotImplementedException();
        }

        public void CreateProjectile(int attackDamage, int projectileCreationCount, Vector2 position, Vector2 direction, float bulletTTL, float bulletSize, IColliderManager colliderManager, ICollection<INonPlayerCharacter> enemyList)
        {
            throw new NotImplementedException();
        }

        public void CreateTriggerZone()
        {
            throw new NotImplementedException();
        }
    }
}
