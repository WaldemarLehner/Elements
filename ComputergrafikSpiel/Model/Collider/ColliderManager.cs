using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Collider
{
    /// <summary>
    /// The Collider Manager stores a reference to all collidable entities and world tiles. Each Scene should have a collider manager.
    /// </summary>
    internal class ColliderManager : IColliderManager
    {
        private readonly int tileSize;

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

        internal ColliderManager(int tileSize)
        {
            if (tileSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tileSize), "Argument needs to be positive");
            }

            this.tileSize = tileSize;
            this.collidableEntities = new List<ICollidable>();
            this.collidableTiles = new Dictionary<Tuple<int, int>, ICollidable>();
        }

        public IReadOnlyCollection<ICollidable> CollidableEntitiesCollection => this.collidableEntities;

        public IReadOnlyDictionary<Tuple<int, int>, ICollidable> CollidableTileDictionary => this.collidableTiles;

        public void AddEntityCollidable(ICollidable collidable)
        {
            this.collidableEntities.Add(collidable);
        }

        public void AddWorldTileCollidable(int x, int y, ICollidable collidable)
        {
            this.collidableTiles[new Tuple<int, int>(x, y)] = collidable;
        }

        public void RemoveEntityCollidable(ICollidable collidable)
        {
            this.collidableEntities.Remove(collidable);
        }

        public void RemoveWorldTileCollidable(int x, int y)
        {
            this.collidableTiles.Remove(new Tuple<int, int>(x, y));
        }

        public void ClearAll()
        {
            this.ClearWorldTileColliders();
            this.ClearEntityColliders();
        }

        public void ClearWorldTileColliders()
        {
            this.collidableTiles.Clear();
        }

        public void ClearEntityColliders()
        {
            this.collidableEntities.Clear();
        }

        public IReadOnlyCollection<ICollidable> GetCollisions(ICollidable collidable)
        {
            List<ICollidable> collidedCollidables = new List<ICollidable>();

            // Static Check : Get The Colliders Maximum Distance from the Colliders Position,
            // then apply a Static Collision Check only to Blocks in the Area of Maximum Distance
            var staticTilesToCheck = this.GetAffectedStaticTiles(collidable.Collider.Position, collidable.Collider.MaximumDistanceFromPosition);
            foreach (var position in staticTilesToCheck)
            {
                var collidableToCheck = this.collidableTiles[position];
                if (collidable.Collider.DidCollideWith(collidableToCheck.Collider) && collidable.Collider != collidableToCheck.Collider)
                {
                    collidedCollidables.Add(collidableToCheck);
                }
            }

            // Check entities
            var dynamicCollidablesToCheck = from entity in this.collidableEntities where Vector2.Distance(entity.Collider.Position, collidable.Collider.Position) - entity.Collider.MaximumDistanceFromPosition - collidable.Collider.MaximumDistanceFromPosition < 0 select entity;
            foreach (var collidableToCheck in dynamicCollidablesToCheck)
            {
                if (collidable.Collider.DidCollideWith(collidableToCheck.Collider) && collidable.Collider != collidableToCheck.Collider)
                {
                    collidedCollidables.Add(collidableToCheck);
                }
            }

            return collidedCollidables;
        }

        public IReadOnlyCollection<ICollidable> GetRayCollisions(IRay ray)
        {
            List<ICollidable> collidedCollidables = new List<ICollidable>();

            this.GetRayCollisionsWithStatic(ray, collidedCollidables);
            this.GetRayCollisionsWithDynamic(ray, collidedCollidables);

            return collidedCollidables;
        }

        internal IEnumerable<Tuple<int, int>> GetAffectedStaticTiles(Vector2 position, float maxDistance)
        {
            // Use Box Distance instead of Radius first, then iterate through that Set to get a subset with fitting Radius
            int minX = (int)Math.Floor((position.X - maxDistance) / this.tileSize);
            int minY = (int)Math.Floor((position.Y - maxDistance) / this.tileSize);
            int maxX = (int)Math.Ceiling((position.X + maxDistance) / this.tileSize);
            int maxY = (int)Math.Ceiling((position.Y + maxDistance) / this.tileSize);

            var boxedKeys = from key
                            in this.collidableTiles.Keys
                            where (key.Item1 >= minX || key.Item1 <= maxX) && (key.Item2 >= minY || key.Item2 <= maxY)
                            select key;

            return from key in boxedKeys where (Vector2.Distance(position, this.collidableTiles[key].Collider.Position) - this.collidableTiles[key].Collider.MaximumDistanceFromPosition) <= maxDistance select key;
        }

        internal IReadOnlyCollection<ICollidable> GetRayCollisionsWithStatic(IRay ray, List<ICollidable> collidedCollidables)
        {
            // currently slow with O(n), will have to be optimized later on
            foreach (KeyValuePair<Tuple<int, int>, ICollidable> tile in this.collidableTiles)
            {
                // compare min distance of the Tile's position and Ray with the "radius" of the tile
                if (tile.Value.Collider.MaximumDistanceFromPosition >= ray.MinimalDistanceTo(tile.Value.Position))
                {
                    collidedCollidables.Add(tile.Value);
                }
            }

            return collidedCollidables;
        }

        internal IReadOnlyCollection<ICollidable> GetRayCollisionsWithDynamic(IRay ray, List<ICollidable> collidedCollidables)
        {
            foreach (var entity in this.collidableEntities)
            {
                // compare min distance of the Entity's position and Ray with the "radius" of the Entity
                if (entity.Collider.MaximumDistanceFromPosition >= ray.MinimalDistanceTo(entity.Position))
                {
                    collidedCollidables.Add(entity);
                }
            }

            return collidedCollidables;
        }
    }
}
