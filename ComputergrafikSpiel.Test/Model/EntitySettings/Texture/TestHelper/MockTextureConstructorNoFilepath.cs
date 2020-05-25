using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Test.Model.EntitySettings.Texture.TestHelper
{
    public class MockTextureConstructorNoFilepath : ITextureContructor
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public string FilePath => "TEST_FILEPATH";
    }
}
