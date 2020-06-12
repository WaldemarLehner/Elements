using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.World.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.World
{
    public class WorldTile : IWorldTile
    {
        internal WorldTile(float size, (int x, int y) gridPos, (Type, SubType) type)
        {
            this.Size = size;
            this.GridPosition = gridPos;
            this.Position = (new Vector2(this.GridPosition.x, this.GridPosition.y) * this.Size) + (Vector2.One * this.Size / 2f);
            this.TileType = type;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Names self-explanatory")]
        public enum Type
        {
            Water,
            Grass,
            Dirt,
            Wall,
            WallTrim,
            Error = 255,
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Names self-explanatory")]
        public enum SubType
        {
            Center,
            EdgeTop,
            EdgeLeft,
            EdgeRight,
            EdgeBottom,
            CornerTopLeft,
            CornerBottomLeft,
            CornerTopRight,
            CornerBottomRight,
            InverseCornerTopLeft,
            InverseCornerTopRight,
            InverseCornerBottomLeft,
            InverseCornerBottomRight,
            NarrowSingle,
            NarrowEndRight,
            NarrowEndLeft,
            NarrowEndTop,
            NarrowEndBottom,
            Top_Bottom,
            Left_Right,
            NarrowCenter,
            Top_Left,
            Top_Right,
            Bottom_Right,
            Bottom_Left,
            Top_TopLeft,
            Top_TopRight,
            Bottom_BottomLeft,
            Bottom_BottomRight,
            Left_BottomLeft,
            Left_TopLeft,
            Right_BottomRight,
            Right_RightTop,
            LeftColumn,
            TopRow,
            BottomRow,
            RightColumn,
            Left_Top_Right,
            Left_Bottom_Right,
            Top_Right_Bottom,
            Top_Left_Bottom,
            Error = 255,
        }

        public (int x, int y) GridPosition { get; }

        public ITexture Texture => throw new NotImplementedException();

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => new List<(Color4 color, Vector2[] vertices)>();

        public Vector2 Position { get; }

        public float Rotation => 0f;

        public Vector2 RotationAnker => this.Position;

        public Vector2 Scale => Vector2.One * this.Size / 2f;

        public (Type, SubType) TileType { get; }

        private float Size { get; }
    }
}
