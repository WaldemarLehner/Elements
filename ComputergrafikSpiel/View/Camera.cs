﻿using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.Scene;
using ComputergrafikSpiel.View.Exceptions;
using ComputergrafikSpiel.View.Helpers;
using ComputergrafikSpiel.View.Renderer.Interfaces;
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
            Scene.ChangeScene += this.Scene_ChangeScene;
        }

        public IRenderer Parent { get; private set; }

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

        public Vector2 WorldToNDC(Vector2 world) => CameraCoordinateConversionHelper.WorldToNDC(world, CameraCoordinateConversionHelper.CalculateAspectRatioMultiplier(this.Parent), this);

        public Vector2 NDCToWorld(Vector2 ndc) => CameraCoordinateConversionHelper.NDCToWorld(ndc, CameraCoordinateConversionHelper.CalculateAspectRatioMultiplier(this.Parent), this);

        public bool AttachRenderer(IRenderer renderer)
        {
            if (this.Parent == null)
            {
                this.Parent = renderer;
                return true;
            }

            return false;
        }

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

        public void DrawRectangle(Rectangle vertices, TextureCoordinates texCoords, (int width, int height) screen)
        {
            var multipliers = CameraCoordinateConversionHelper.CalculateAspectRatioMultiplier(this.AspectRatio, screen.width / (float)screen.height);
            (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) ndcVertices = this.GenerateNDCVertices(vertices, multipliers);
            var vertTexPair = this.GenerateNDCVertex_TexCollection(ndcVertices, texCoords);

            this.DrawPrimitive(vertTexPair, PrimitiveType.Quads);
        }

        public void DrawAsBackground(Rectangle alignedItem, (int width, int height) screen)
        {
            var multipliers = CameraCoordinateConversionHelper.CalculateAspectRatioMultiplier(this.AspectRatio, screen.width / (float)screen.height);
            (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) ndcVertices = this.GenerateNDCVertices(alignedItem, multipliers);

            // Map Vertex-Coordinates as TextureCoordinate Inverse
            var (tl, tr, br, bl, coords) = this.CalculateBackgroundVertTexTuple(ndcVertices);

            var position = (tl, tr, br, bl);
            this.DrawPrimitive(this.GenerateNDCVertex_TexCollection(position, coords), PrimitiveType.Quads);
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

        private (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL, TextureCoordinates coords) CalculateBackgroundVertTexTuple((Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) ndcVertices)
        {
            float width = Math.Abs(ndcVertices.TL.X - ndcVertices.TR.X);
            float height = Math.Abs(ndcVertices.TL.Y - ndcVertices.BL.Y);

            var xCount = (int)(2 / width) + 5;
            var yCount = (int)(2 / height) + 5;

            Vector2 anker = new Vector2(ndcVertices.TL.X % width, ndcVertices.TL.Y % height);

            var xHalf = (float)Math.Ceiling((double)xCount / 2f);
            var yHalf = (float)Math.Ceiling((double)yCount / 2f);

            var tl = new Vector2(-xHalf * width, yHalf * height) + anker;
            var tr = new Vector2(xHalf * width, yHalf * height) + anker;
            var br = new Vector2(xHalf * width, -yHalf * height) + anker;
            var bl = new Vector2(-xHalf * width, -yHalf * height) + anker;
            var tex = new TextureCoordinates(new Vector2(-xHalf, yHalf), new Vector2(xHalf, yHalf), new Vector2(xHalf, -yHalf), new Vector2(-xHalf, -yHalf));
            return (tl, tr, br, bl, tex);
        }

        private (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) GenerateNDCVertices(Rectangle worldSpaceVerts, (float x, float y) multipliers)
        {
            return (
                CameraCoordinateConversionHelper.WorldToNDC(worldSpaceVerts.TopLeft, multipliers, this),
                CameraCoordinateConversionHelper.WorldToNDC(worldSpaceVerts.TopRight, multipliers, this),
                CameraCoordinateConversionHelper.WorldToNDC(worldSpaceVerts.BottomRight, multipliers, this),
                CameraCoordinateConversionHelper.WorldToNDC(worldSpaceVerts.BottomLeft, multipliers, this));
        }

        private ICollection<(Vector2 vert, Vector2 tex)> GenerateNDCVertex_TexCollection((Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) ndcVert, TextureCoordinates tex)
        {
            return new List<(Vector2 vert, Vector2 tex)>()
            {
                (ndcVert.TL, tex.TopLeft),
                (ndcVert.TR, tex.TopRight),
                (ndcVert.BR, tex.BottomRight),
                (ndcVert.BL, tex.BottomLeft),
            };
        }

        private void Scene_ChangeScene(object sender, EventArgs e)
        {
            var scene = sender as Scene;
            var (top, bottom, left, right) = scene.World.WorldSceneBounds;
            this.Update(top, bottom, left, right);
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

        private void DrawPrimitive(IEnumerable<(Vector2 vert, Vector2 tex)> data, PrimitiveType primitiveType)
        {
            GL.Begin(primitiveType);
            foreach (var (vert, tex) in data)
            {
                GL.TexCoord2(tex);
                GL.Vertex2(vert);
            }

            GL.End();
        }
    }
}
