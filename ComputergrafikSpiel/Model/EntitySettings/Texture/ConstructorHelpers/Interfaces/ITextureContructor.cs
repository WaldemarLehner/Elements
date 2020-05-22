using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces
{
    internal interface ITextureContructor
    {
        int Width { get; }

        int Height { get; }

        string FilePath { get; }
    }
}
