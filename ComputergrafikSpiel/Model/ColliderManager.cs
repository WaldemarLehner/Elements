using System;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Model
{
    /// <summary>
    /// The Collider Manager stores a reference to all collidable entities and world tiles. Each Scene should have a collider manager.
    /// </summary>
    internal class ColliderManager
    {
        /// <summary>
        /// List of Dynamic Collidables. For now we will use a "stupid" approach of just iterating through each Collider and checking if
        /// there is a collision. This takes O(n) to check one object, and O(n²) to check all objects. A quad-tree shall be considered for the
        /// future, reducing big O to O(log n) for one object and O(n log n) for all objects.
        /// </summary>
        private readonly List<ICollidable> collidableEntities;

        /// <summary>
        /// A Tuple which assigns a Static Collider to a World Tile Coordinate.
        /// That way checking collision takes O(1) for static objects.
        /// </summary>
        private Dictionary<Tuple<int, int>, ICollidable> collidableTiles;

        internal ColliderManager()
        {
            this.collidableEntities = new List<ICollidable>();
            this.collidableTiles = new Dictionary<Tuple<int, int>, ICollidable>();
        }

        internal IReadOnlyCollection<ICollidable> CollidableEntitiesCollection => this.collidableEntities;

        internal IReadOnlyDictionary<Tuple<int, int>, ICollidable> CollidableTilesDictionary => this.collidableTiles;

        internal void AddEntity(ICollidable collidable)
        {
            this.collidableEntities.Add(collidable);
        }

        internal void AddWorldTile(int x, int y, ICollidable collidable)
        {
            this.collidableTiles[new Tuple<int, int>(x, y)] = collidable;
        }

        internal void RemoveEntity(ICollidable collidable)
        {
            this.collidableEntities.Remove(collidable);
        }

        internal void RemoveWorldTileCollider(int x, int y)
        {
            this.collidableTiles.Remove(new Tuple<int, int>(x, y));
        }

        internal void ClearAll()
        {
            this.ClearWorldTileColliders();
            this.ClearEntityColliders();
        }

        internal void ClearWorldTileColliders()
        {
            foreach (var key in this.collidableTiles.Keys)
            {
                this.collidableTiles.Remove(key);
            }
        }

        internal void ClearEntityColliders()
        {
            this.collidableEntities.Clear();
        }

        internal IReadOnlyCollection<ICollidable> GetCollisions(ICollidable collidable)
        {
            List<ICollidable> collidedCollidables = new List<ICollidable>();
            throw new NotImplementedException();
        }
    }
}
