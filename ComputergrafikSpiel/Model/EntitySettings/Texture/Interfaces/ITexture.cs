namespace ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces
{
    internal interface ITexture // : IUpdateable NOCH NICHT IMPLEMENTIERT
    {
        int Width { get; }

        int Height { get; }

        string FilePath { get; }
    }
}