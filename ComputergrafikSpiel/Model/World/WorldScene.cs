﻿using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.World.Interfaces;

namespace ComputergrafikSpiel.Model.World
{
    internal class WorldScene : IWorldScene
    {
        internal WorldScene(IWorldSceneDefinition definition, IWorldTile[,] tiles, IWorldObstacle[] obstacles)
        {
            this.SceneDefinition = definition ?? throw new ArgumentNullException(nameof(definition));
            this.WorldTiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
            this.Obstacles = (from o in obstacles orderby o.Position.Y descending select o).ToList();
            var list = new List<IWorldTile>();
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    list.Add(tiles[x, y]);
                }
            }

            this.WorldTilesEnumerable = list;
        }

        public IWorldSceneDefinition SceneDefinition { get; }

        public IWorldTile[,] WorldTiles { get; }

        public List<IWorldObstacle> Obstacles { get; }

        public IEnumerable<IWorldTile> WorldTilesEnumerable { get; }

        public (float top, float bottom, float left, float right) WorldSceneBounds
        {
            get
            {
                var top = (this.SceneDefinition.TileCount.y + (1f / 4f) + 2) * this.SceneDefinition.TileSize;
                var bottom = this.SceneDefinition.TileSize * (3f / 4f);
                var left = this.SceneDefinition.TileSize * 3f / 4f;
                var right = (this.SceneDefinition.TileCount.x + (1f / 4f) + 1) * this.SceneDefinition.TileSize;
                return (top, bottom, left, right);
            }
        }

        IEnumerable<IWorldObstacle> IWorldScene.Obstacles => this.Obstacles;
    }
}
