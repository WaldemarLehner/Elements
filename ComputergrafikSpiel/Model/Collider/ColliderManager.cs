using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Triggers.Interfaces;
using OpenTK;
using OpenTK.Graphics;

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
        private readonly Dictionary<Tuple<int, int>, ICollidable> collidableTiles;

        private readonly Dictionary<Tuple<int, int>, ITrigger> collidableTriggers;

        internal ColliderManager(int tileSize)
        {
            if (tileSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tileSize), "Argument needs to be positive");
            }

            Console.WriteLine("new ColliderManager");

            this.tileSize = tileSize;
            this.collidableEntities = new List<ICollidable>();
            this.collidableTiles = new Dictionary<Tuple<int, int>, ICollidable>();
            this.collidableTriggers = new Dictionary<Tuple<int, int>, ITrigger>();
        }

        public IReadOnlyCollection<ICollidable> CollidableEntitiesCollection => this.collidableEntities;

        public IReadOnlyDictionary<Tuple<int, int>, ICollidable> CollidableTileDictionary => this.collidableTiles;

        public IReadOnlyDictionary<Tuple<int, int>, ITrigger> CollidableTriggerDictionary => this.collidableTriggers;

        public void AddEntityCollidable(ICollidable collidable)
        {
            this.collidableEntities.Add(collidable);
        }

        public void AddWorldTileCollidable(int x, int y, ICollidable collidable)
        {
            this.collidableTiles[new Tuple<int, int>(x, y)] = collidable;
        }

        public void AddTriggerCollidable(int x, int y, ITrigger trigger)
        {
            this.collidableTriggers[new Tuple<int, int>(x, y)] = trigger;
        }

        public void RemoveEntityCollidable(ICollidable collidable)
        {
            this.collidableEntities.Remove(collidable);
        }

        public void RemoveWorldTileCollidable(int x, int y)
        {
            this.collidableTiles.Remove(new Tuple<int, int>(x, y));
        }

        public void RemoveTriggerCollidable(int x, int y)
        {
            this.collidableTriggers.Remove(new Tuple<int, int>(x, y));
        }

        public void ClearAll()
        {
            this.ClearWorldTileColliders();
            this.ClearEntityColliders();
            this.ClearTriggerColliders();
        }

        public void ClearWorldTileColliders()
        {
            this.collidableTiles.Clear();
        }

        public void ClearEntityColliders()
        {
            this.collidableEntities.Clear();
        }

        public void ClearTriggerColliders()
        {
            this.collidableTriggers.Clear();
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

        public void HandleTriggerCollisions(IPlayer player)
        {
            foreach (var trigger in this.collidableTriggers.ToList())
            {
                if (player.Collider.DidCollideWith(trigger.Value.Collider))
                {
                    trigger.Value.TriggerCollisionFunction();
                }
            }
        }

        public IReadOnlyCollection<ICollidable> GetRayCollisions(IRay ray)
        {
            var @static = this.GetRayCollisionsWithStatic(ray);
            var @dynamic = this.GetRayCollisionsWithDynamic(ray);
            var union = Enumerable.Union(@static, dynamic).ToList();
            Scene.Scene.Current.IndependentDebugData.Add((new Color4(255, 0, 0, 255), new Vector2[] { ray.Position, ray.Position + (ray.Direction.Normalized() * ray.MaxDistance) }));
            return union;
        }

        public IReadOnlyCollection<ICollidable> GetRayCollisionsSorted(IRay ray, Vector2 position)
        {
            var unsorted = this.GetRayCollisions(ray);
            return (from entry in unsorted orderby Vector2.DistanceSquared(position, entry.Collider.Position) ascending select entry).ToList();
        }

        internal IEnumerable<Tuple<int, int>> GetAffectedStaticTiles(Vector2 position, float maxDistance)
        {
            var tileRadius = (int)Math.Ceiling(maxDistance / this.tileSize) + 1;
            var tileRadiusSquared = tileRadius * tileRadius;
            var tileOfPosition = (x: (int)(position.X / this.tileSize), y: (int)(position.Y / this.tileSize));

            (int lower, int upper) x = (tileOfPosition.x - tileRadius, tileOfPosition.x + tileRadius);
            (int lower, int upper) y = (tileOfPosition.y - tileRadius, tileOfPosition.y + tileRadius);

            var filteredBox = this.CollidableTileDictionary.Keys.Where((Tuple<int, int> e) => e.Item1 > x.lower && e.Item1 < x.upper && e.Item2 > y.lower && e.Item2 < y.upper);
            return filteredBox.Where((Tuple<int, int> e) => TileDistanceSquared(e.Item1, e.Item2) < tileRadiusSquared);

            float TileDistanceSquared(int x_, int y_) => ((tileOfPosition.x - x_) * (tileOfPosition.x - x_)) + ((tileOfPosition.y - y_) * (tileOfPosition.y - y_));
        }

        internal IReadOnlyCollection<ICollidable> GetRayCollisionsWithStatic(IRay ray)
        {
            var collidedCollidables = new List<ICollidable>();

            // currently slow with O(n), will have to be optimized later on
            foreach (KeyValuePair<Tuple<int, int>, ICollidable> tile in this.collidableTiles)
            {
                if (ray.DidCollideWith(tile.Value.Collider))
                {
                    collidedCollidables.Add(tile.Value);
                }
            }

            return collidedCollidables;
        }

        internal IReadOnlyCollection<ICollidable> GetRayCollisionsWithDynamic(IRay ray)
        {
            var collidedCollidables = new List<ICollidable>();
            foreach (var entity in this.collidableEntities)
            {
                // compare min distance of the Entity's position and Ray with the "radius" of the Entity
                if (ray.DidCollideWith(entity.Collider))
                {
                    collidedCollidables.Add(entity);
                }
            }

            return collidedCollidables;
        }

        internal IReadOnlyCollection<ICollidable> GetRayCollisionsWithDynamic(IRay ray, Vector2 initialPosition)
        {
            var collidedCollidables = this.GetRayCollisionsWithDynamic(ray);
            return (from entry in collidedCollidables orderby Vector2.DistanceSquared(entry.Collider.Position, initialPosition) ascending select entry).ToList();
        }
    }
}
