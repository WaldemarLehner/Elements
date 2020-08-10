﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.World;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Overlay
{
    internal static class GUIConstructionHelper
    {
        private static readonly ITileTexture Instruction = new TextureLoader().LoadTileTexture("GUI/Instruction", (x: 1, y: 1));
        private static readonly IMappedTileFont Font = new TextureLoader().LoadFontTexture("Font/vt323", (x: 8, y: 8), FontTextureMappingHelper.Default);
        private static readonly ITileTexture Heart = new TextureLoader().LoadTileTexture("GUI/Heart", (x: 2, y: 1));
        private static readonly ITileTexture Crosshair = new TextureLoader().LoadTileTexture("GUI/Crosshair_Cursor", (x: 1, y: 1));
        private static readonly ITileTexture Gameover = new TextureLoader().LoadTileTexture("GUI/gameover", (x: 1, y: 1));

        public static List<IRenderable> GenerateInstruction(IWorldScene sceneDefinition)
        {
            List<IRenderable> healthEntries = new List<IRenderable>();

            // Get bounds of GUI Area of Scene.
            float left = sceneDefinition.WorldSceneBounds.left;
            float right = sceneDefinition.WorldSceneBounds.right;
            float top = sceneDefinition.WorldSceneBounds.top;
            float bottom = sceneDefinition.WorldSceneBounds.bottom;

            float instructionSize = (right - left) / 2;

            if ((Scene.Scene.Current.Model as Model).SceneManager.CurrentStageLevel == 0)
            {
                float xCenter = (left + right) / 2;
                float yCenter = (bottom + top) / 2;
                var texCoords = Instruction.GetTexCoordsOfIndex(0); // Für die rechte Hälfte des Bildes.

                var entry = new GenericGUIRenderable()
                {
                    Scale = new Vector2(instructionSize * .95f, .5f * instructionSize), //Vector2.One * .5f * instructionSize,
                    Position = new Vector2(xCenter, yCenter),
                    Texture = Instruction,
                    Coordinates = texCoords,
                };
                healthEntries.Add(entry);
            }

            return healthEntries;
        }

        internal static IEnumerable<IRenderable> GenerateGuiIndicator(IWorldScene sceneDefinition, IPlayer player)
        {
            var dungeonInfo = GUIConstructionHelper.GenerateDungeonInfo(sceneDefinition);
            var coinData = GUIConstructionHelper.GenerateCoinCount(sceneDefinition, player);
            var healthbar = GUIConstructionHelper.GenerateHealthBar(sceneDefinition, player);
            var gameover = GUIConstructionHelper.GenerateGameover(sceneDefinition, player);
            var crosshair = GUIConstructionHelper.GenerateCrosshair();
            var renderables = new List<IRenderable>();

            renderables.AddRange(dungeonInfo);
            renderables.AddRange(coinData);
            renderables.AddRange(healthbar);
            renderables.AddRange(gameover);
            renderables.AddRange(crosshair);
            return renderables;
        }

        private static List<IRenderable> GenerateDungeonInfo(IWorldScene sceneDefinition)
        {
            // Get bounds of GUI Area of Scene.
            (float top, float bottom, float left, _, float width, float height) = GenerateCoinGUIBounds(sceneDefinition.SceneDefinition.TileSize, sceneDefinition.WorldSceneBounds);
            string dungeonInfo;
            if (Scene.Scene.Current.Model.SceneManager.CurrentDungeon == 0)
            {
                dungeonInfo = "safezone";
            }
            else
            {
                dungeonInfo = "dungeon: " + Scene.Scene.Current.Model.SceneManager.CurrentDungeon.ToString() + "  room: " + Scene.Scene.Current.Model.SceneManager.CurrentDungeonRoom.ToString();
            }

            int renderablesCount = dungeonInfo.Length;
            float itemSize = (width / renderablesCount) < height ? width / renderablesCount : height;

            List<IRenderable> renderables = new List<IRenderable>();

            for (int i = 0; i < renderablesCount; i++)
            {
                var tex = Font;
                int? texIndex = GetTexIndex(tex, dungeonInfo[i]);
                if (texIndex == null)
                {
                    continue;
                }

                var entry = new GenericGUIRenderable()
                {
                    Scale = Vector2.One * itemSize / .8f,
                    Position = new Vector2(left + ((i * (itemSize / .6f)) - 180), (top + bottom) / 2f),
                    Texture = tex,
                    Coordinates = tex.GetTexCoordsOfIndex((int)texIndex),
                };
                renderables.Add(entry);
            }

            return renderables;
        }

        private static List<IRenderable> GenerateCoinCount(IWorldScene sceneDefinition, IPlayer player)
        {
            // Get bounds of GUI Area of Scene.
            (float top, float bottom, float left, _, float width, float height) = GenerateCoinGUIBounds(sceneDefinition.SceneDefinition.TileSize, sceneDefinition.WorldSceneBounds);

            string moneyCount = player.PlayerData.currency.ToString() + "$";
            int renderablesCount = moneyCount.Length;
            float itemSize = (width / renderablesCount) < height ? width / renderablesCount : height;

            List<IRenderable> renderables = new List<IRenderable>();

            for (int i = 0; i < renderablesCount; i++)
            {
                var tex = Font;
                int? texIndex = GetTexIndex(tex, moneyCount[i]);
                if (texIndex == null)
                {
                    continue;
                }

                var entry = new GenericGUIRenderable()
                {
                    Scale = Vector2.One * itemSize / 2f,
                    Position = new Vector2(left + ((i + .5f) * itemSize), (top + bottom) / 2f),
                    Texture = tex,
                    Coordinates = tex.GetTexCoordsOfIndex((int)texIndex),
                };
                renderables.Add(entry);
            }

            return renderables;
        }

        private static (float top, float bottom, float left, float right, float width, float height) GenerateCoinGUIBounds(int tileSize, (float top, float bottom, float left, float right) worldSceneBounds)
        {
            float left = worldSceneBounds.right - (tileSize * 3);
            float right = worldSceneBounds.right;
            float top = worldSceneBounds.top;
            float bottom = top - tileSize;
            float width = right - left;
            float height = top - bottom;

            return (top, bottom, left, right, width, height);
        }

        private static int? GetTexIndex(IMappedTileFont tex, char v)
        {
            var (x, y) = tex.GetTileOfKey(v);
            if (x == -1 && y == -1)
            {
                return null;
            }

            return (y * tex.XRows) + x;
        }

        private static List<IRenderable> GenerateHealthBar(IWorldScene sceneDefinition, IPlayer player)
        {
            List<IRenderable> healthEntries = new List<IRenderable>();
            var (currentHealth, maxHealth, _, _, _) = player.PlayerData;

            // Get bounds of GUI Area of Scene.
            float left = sceneDefinition.WorldSceneBounds.left;
            float right = sceneDefinition.WorldSceneBounds.right - (sceneDefinition.SceneDefinition.TileSize * 3);
            float top = sceneDefinition.WorldSceneBounds.top;
            float bottom = top - sceneDefinition.SceneDefinition.TileSize;

            float heartSize = (right - left) / maxHealth;
            if (heartSize < 0)
            {
                heartSize *= -1;
            }

            if (heartSize > top - bottom)
            {
                heartSize = top - bottom;
            }

            for (int i = 0; i < maxHealth; i++)
            {
                float xCenter = left + ((i + .5f) * heartSize);
                var texCoords = Heart.GetTexCoordsOfIndex((i >= currentHealth) ? 0 : 1); // Get the required texture.

                var entry = new GenericGUIRenderable()
                {
                    Scale = Vector2.One * .5f * heartSize,
                    Position = new Vector2(xCenter, .5f * (top + bottom)),
                    Texture = Heart,
                    Coordinates = texCoords,
                };
                healthEntries.Add(entry);
            }

            return healthEntries;
        }

        private static List<IRenderable> GenerateGameover(IWorldScene sceneDefinition, IPlayer player)
        {
            List<IRenderable> healthEntries = new List<IRenderable>();

            // Get bounds of GUI Area of Scene.
            float left = sceneDefinition.WorldSceneBounds.left;
            float right = sceneDefinition.WorldSceneBounds.right;
            float top = sceneDefinition.WorldSceneBounds.top;
            float bottom = sceneDefinition.WorldSceneBounds.bottom;

            float gameoverSize = (right - left) / 2;
            var (currentHealth, _, _, _, _) = player.PlayerData;

            if (currentHealth == 0 || (((Scene.Scene.Current.Model as Model).SceneManager.CurrentStageLevel == 40) && Scene.Scene.Current.NpcList.Count == 0))
            {
                float xCenter = (left + right) / 2;
                float yCenter = (bottom + top) / 1.7f;
                var texCoords = Gameover.GetTexCoordsOfIndex(0); // Für die rechte Hälfte des Bildes.

                var entry = new GenericGUIRenderable()
                {
                    Scale = Vector2.One * .5f * gameoverSize,
                    Position = new Vector2(xCenter, yCenter),
                    Texture = Gameover,
                    Coordinates = texCoords,
                };
                healthEntries.Add(entry);
            }

            return healthEntries;
        }

        private static List<IRenderable> GenerateCrosshair()
        {
            List<IRenderable> crosshairEntries = new List<IRenderable>();

            float croshairSize = 15;
            for (int i = 0; i < 1; i++)
            {
                var texCoords = Crosshair.GetTexCoordsOfIndex(0); // Get the required texture.
                var entry = new GenericGUIRenderable()
                {
                    Scale = Vector2.One * .5f * croshairSize,
                    Position = Scene.Scene.Current?.Model?.InputState?.Cursor?.WorldCoordinates ?? Vector2.Zero,
                    Texture = Crosshair,
                    Coordinates = texCoords,
                };
                crosshairEntries.Add(entry);
            }

            return crosshairEntries;
        }

        private class GenericGUIRenderable : IRenderableLayeredTextures
        {
            public ITexture Texture { get; set; }

            public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

            public Vector2 Position { get; set; }

            public float Rotation => 0f;

            public Vector2 RotationAnker => this.Position;

            public Vector2 Scale { get; set; }

            (IEnumerable<TextureCoordinates>, ITileTexture) IRenderableLayeredTextures.Texture => (new TextureCoordinates[] { this.Coordinates }, this.Texture as ITileTexture);

            internal TextureCoordinates Coordinates { get; set; }
        }
    }
}
