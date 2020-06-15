using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Trigger.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Collider.Interfaces
{
    public interface IColliderManager
    {
        IReadOnlyCollection<ICollidable> CollidableEntitiesCollection { get; }

        IReadOnlyDictionary<Tuple<int, int>, ICollidable> CollidableTileDictionary { get; }

        void AddEntityCollidable(ICollidable collidable);

        void AddWorldTileCollidable(int x, int y, ICollidable collidable);

        void AddTriggerCollidable(int x, int y, ITrigger trigger);

        void RemoveEntityCollidable(ICollidable collidable);

        void RemoveWorldTileCollidable(int x, int y);

        void RemoveTriggerCollidable(int x, int y);

        void ClearAll();

        void ClearTriggerColliders();

        void ClearWorldTileColliders();

        void ClearEntityColliders();

        IReadOnlyCollection<ICollidable> GetCollisions(ICollidable collidable);

        void HandleTriggerCollisions(IPlayer player);

        IReadOnlyCollection<ICollidable> GetRayCollisions(IRay ray);

        IReadOnlyCollection<ICollidable> GetRayCollisions(IRay ray, Vector2 position);
    }
}