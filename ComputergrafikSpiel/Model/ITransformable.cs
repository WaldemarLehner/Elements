namespace ComputergrafikSpiel.Model
{
    internal interface ITransformable
    {
        ITransformable Position { get; }

        IRotatable Rotate { get; }

        IScalable Scale { get; }
    }
}