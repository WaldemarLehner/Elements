using OpenTK;

namespace ComputergrafikSpiel.Controller.Input
{
    public interface IMouseCursor
    {
        Vector2 WindowNDCCoordinates { get; }

        Vector2? WorldCoordinates { get; }
    }
}