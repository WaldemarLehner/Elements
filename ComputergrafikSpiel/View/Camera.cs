using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using ComputergrafikSpiel.View.Exceptions;
using ComputergrafikSpiel.View.Helpers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.View
{
    internal class Camera : Interfaces.ICamera
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// Creates a new Camera to be used by the Renderer to define bounds.
        /// </summary>
        /// <param name="top">The y-Coordinate defining the top bound in geometry-Coordinates.</param>
        /// <param name="bottom">The y-Coordinate defining the bottom bound in geometry-Coordinates.</param>
        /// <param name="left">The x-Coordinate defining the left bound in geometry-Coordinates.</param>
        /// <param name="right">The x-Coordinate defining the right bound in geometry-Coordinates.</param>
        internal Camera(float top, float bottom, float left, float right)
        {
            this.Update(top, bottom, left, right);
        }

        public float Width => this.Right - this.Left;

        public float Height => this.Top - this.Bottom;

        public float AspectRatio => this.Width / this.Height;

        public float Top { get; private set; }

        public float Bottom { get; private set; }

        public float Left { get; private set; }

        public float Right { get; private set; }

        public Vector2 TopLeft => new Vector2(this.Left, this.Top);

        public Vector2 TopRight => new Vector2(this.Right, this.Top);

        public Vector2 BottomLeft => new Vector2(this.Left, this.Bottom);

        public Vector2 BottomRight => new Vector2(this.Right, this.Bottom);

        public (Vector2 TL, Vector2 TR, Vector2 BL, Vector2 BR) CameraBounds => (TL: this.TopLeft, TR: this.TopRight, BL: this.BottomLeft, BR: this.BottomRight);

        public bool CanPointBeSeenByCamera(Vector2 point)
        {
            if (point.X < this.Left || point.X > this.Right)
            {
                return false;
            }

            if (point.Y < this.Bottom || point.Y > this.Top)
            {
                return false;
            }

            return true;
        }

        public void DrawRectangle(Rectangle vertices, (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) texCoords, (int width, int height) screen)
        {
            var multipliers = CameraCoordinateConversionHelper.CalculateAspectRatioMultiplier(this.AspectRatio, screen.width / (float)screen.height);
            (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) ndcVertices = this.GenerateNDCVertices(vertices, multipliers);
            var vertTexPair = this.GenerateNDCVertex_TexCollection(ndcVertices, texCoords);

            this.DrawPrimitive(vertTexPair, PrimitiveType.Quads);
        }

        /// <summary>
        /// Converts a <see cref="Vector2"/> from Screen Space to World Space.
        /// </summary>
        /// <param name="screenSpaceCoord">A Screen Space Coordinate x {0;1} , y {0;1} ; Origin: Bottom Left.</param>
        /// <param name="screen"> Screen Dimensions.</param>
        /// <returns>A <see cref="Vector2"/> in World Space Coordinates.</returns>
        public Vector2 GetWorldCoordinateFromScreenSpace(Vector2 screenSpaceCoord, (int width, int height) screen)
        {
            this.ConstructorCheckWorldCoordinatesBounds(screenSpaceCoord, screen);
            var multipliers = CameraCoordinateConversionHelper.CalculateAspectRatioMultiplier(this.AspectRatio, screen.width / (float)screen.height);

            return CameraCoordinateConversionHelper.NDCToWorld(screenSpaceCoord, multipliers, this);
        }

        public void Update(float top, float bottom, float left, float right)
        {
            this.UpdateCheck(top, bottom, left, right);

            this.Top = top;
            this.Bottom = bottom;
            this.Left = left;
            this.Right = right;
        }

        private (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) GenerateNDCVertices(Rectangle worldSpaceVerts, (float x, float y) multipliers)
        {
            return (
                CameraCoordinateConversionHelper.WorldToNDC(worldSpaceVerts.TopLeft, multipliers, this),
                CameraCoordinateConversionHelper.WorldToNDC(worldSpaceVerts.TopRight, multipliers, this),
                CameraCoordinateConversionHelper.WorldToNDC(worldSpaceVerts.BottomRight, multipliers, this),
                CameraCoordinateConversionHelper.WorldToNDC(worldSpaceVerts.BottomLeft, multipliers, this));
        }

        // WARN: TODO: For some reason having the "correct" Tex Coords results in orientation, namely 90deg clockwise. This is why they had to be swapped
        private ICollection<(Vector2 vert, Vector2 tex)> GenerateNDCVertex_TexCollection((Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) ndcVert, (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) tex)
        {
            return new List<(Vector2 vert, Vector2 tex)>()
            {
                (ndcVert.TL, tex.TR),
                (ndcVert.TR, tex.BR),
                (ndcVert.BR, tex.BL),
                (ndcVert.BL, tex.TL),
            };
        }

        private void ConstructorCheckWorldCoordinatesBounds(Vector2 screenSpaceCoord, (int w, int h) screen)
        {
            if (screenSpaceCoord.X < 0 || screenSpaceCoord.X > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(screenSpaceCoord.X), "X needs to be between 0 and 1");
            }

            if (screenSpaceCoord.Y < 0 || screenSpaceCoord.Y > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(screenSpaceCoord.Y), "Y needs to be between 0 and 1");
            }

            if (screen.w <= 0)
            {
                throw new ArgumentNotPositiveIntegerGreaterZeroException(nameof(screen.w));
            }

            if (screen.h <= 0)
            {
                throw new ArgumentNotPositiveIntegerGreaterZeroException(nameof(screen.h));
            }
        }

        private void UpdateCheck(float top, float bottom, float left, float right)
        {
            if (top - bottom <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(top) + "and " + nameof(bottom), "Top needs to be greater than Bottom");
            }

            if (right - left <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(right) + " and " + nameof(left), "Right needs to be greater than Left");
            }
        }

        private void DrawPrimitive(ICollection<(Vector2 vert, Vector2 tex)> data, PrimitiveType primitiveType)
        {
            GL.Begin(primitiveType);
            foreach (var (vert, tex) in data)
            {
                GL.Vertex2(vert);
                GL.TexCoord2(tex);
            }

            GL.End();
        }
    }
}
