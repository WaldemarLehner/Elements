﻿using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Triggers.Interfaces;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

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
            // is this needed for anything?
            if(collidable == null)
            {
                int x = 0;
            }
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
                    Console.WriteLine("tile hit");
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

            (int x, int y) tileKeys = ((int)(position.X / this.tileSize), (int)(position.Y / this.tileSize));
            int tileDistance = (int)Math.Ceiling(maxDistance / this.tileSize);

            (int lower, int upper) x = (tileKeys.x - tileDistance, tileKeys.x + tileDistance);
            (int lower, int upper) y = (tileKeys.y - tileDistance, tileKeys.y + tileDistance);
            if(x.lower < 0)
            {
                x.lower = 0;
            }
            if (y.lower < 0)
            {
                y.lower = 0;
            }

            // Manager does not store max index.
            var boxedKeys = from key
                            in this.collidableTiles.Keys
                            where (key.Item1 >= x.lower || key.Item1 <= x.upper) && (key.Item2 >= y.lower || key.Item2 <= y.upper)
                            select key;
            return boxedKeys;
            /*
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
    */
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
