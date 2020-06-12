using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Model.World.Interfaces
{
    public interface IWorldScene
    {
        IWorldSceneDefinition SceneDefinition { get; }

        IWorldTile[,] WorldTiles { get; }


    }
}
