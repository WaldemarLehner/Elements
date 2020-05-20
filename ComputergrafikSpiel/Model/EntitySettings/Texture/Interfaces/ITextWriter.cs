using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces
{
    internal interface ITextWriter : IMappedTileFont
    {
        float FontSize { get; }

        float VerticalOffset { get; }

        float HorizontalOffset { get; }

        void Write(string text, Vector2 position, int maxWidthChars); // Noch prüfen welcher Rückgabewert verlangt
    }
}
