using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    public class BackgroundRenderable : IRenderableBackground
    {
        public BackgroundRenderable(ITexture texture, Vector2 position, Vector2 scale, TextureWrapMode textureWrapMode = TextureWrapMode.Repeat)
        {
            this.Texture = texture ?? throw new ArgumentNullException(nameof(texture));
            this.Position = position;
            this.Scale = scale;
            this.WrapMode = textureWrapMode;
        }

        public TextureWrapMode WrapMode { get; }

        public ITexture Texture { get; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

        public Vector2 Position { get; }

        public float Rotation => 0f;

        public Vector2 RotationAnker => this.Position;

        public Vector2 Scale { get; }
    }
}
