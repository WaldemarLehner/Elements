using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;

namespace ComputergrafikSpiel.Test.Model.EntitySettings.Texture.TestHelper
{
    public class MockTextureConstructorNoFilepath : ITextureContructor
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public string FilePath => "TEST_FILEPATH";
    }
}
