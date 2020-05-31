using System;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Model.Collider.Interfaces
{
    public interface IColliderManager
    {
        IReadOnlyCollection<ICollidable> CollidableEntitiesCollection { get; }

        IReadOnlyDictionary<Tuple<int, int>, ICollidable> CollidableTileDictionary { get; }

        void AddEntityCollidable(ICollidable collidable);

        void AddWorldTileCollidable(int x, int y, ICollidable collidable);

        void RemoveEntityCollidable(ICollidable collidable);

        void RemoveWorldTileCollidable(int x, int y);

        void ClearAll();

        void ClearWorldTileColliders();

        void ClearEntityColliders();

        IReadOnlyCollection<ICollidable> GetCollisions(ICollidable collidable);

        IReadOnlyCollection<ICollidable> GetRayCollisions(IRay ray);
    }
}