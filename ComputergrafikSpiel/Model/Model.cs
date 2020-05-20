using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal class Model : IModel
    {
        private float timeSum = 0;

        internal Model()
        {
            this.RenderablesList = new List<IRenderable>();
            this.RenderablesList.Add(new TestRenderable());
        }

        public IReadOnlyCollection<IRenderable> Renderables => this.RenderablesList;

        private List<IRenderable> RenderablesList { get; }

        /// <summary>
        /// For the Test, this will draw a Rectangle doing a loop.
        /// </summary>
        /// <param name="dTime">Time between two Update Calls in Seconds.</param>
        public void Update(float dTime)
        {
            this.timeSum += dTime;
            TestRenderable item = this.RenderablesList.First() as TestRenderable;
            item.Position = this.CalculateCubePosition(0, Vector2.One * 100, 50);
            item.Rotation = this.timeSum / 20;
            item.RotationAnker = item.Position + (new Vector2((float)Math.Sin(this.timeSum), (float)Math.Cos(this.timeSum)) * 20);

            // Console.WriteLine($"<{item.Position.X},{item.Position.Y}> <{item.RotationAnker.X},{item.RotationAnker.Y}>");
        }

        private Vector2 CalculateCubePosition(float timeOffset, Vector2 positionOffset, float radius)
        {
            return (new Vector2((float)Math.Cos(timeOffset + this.timeSum), (float)Math.Sin(timeOffset + this.timeSum)) * radius) + positionOffset;
        }

        private class TestRenderable : IRenderable
        {
            public Vector2 Position { get; set; } = Vector2.Zero;

            public Vector2 Scale { get; set; } = Vector2.One * 20;

            public float Rotation { get; set; } = 0f;

            public Vector2 RotationAnker { get; set; } = Vector2.Zero;

            public ITexture Texture { get; } = null;
        }
    }
}
