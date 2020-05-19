namespace ComputergrafikSpiel.Model
{
    internal interface ITransformable
    {
        IPositionable Position { get; }

        IRotatable Rotate { get; }

        IScalable Scale { get; }
    }
}