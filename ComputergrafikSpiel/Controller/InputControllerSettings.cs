using System.Collections.Generic;
using ComputergrafikSpiel.Model;
using OpenTK.Input;

namespace ComputergrafikSpiel.Controller
{
    public struct InputControllerSettings
    {
        public Dictionary<Key, PlayerActionEnum.PlayerActions> KeyboardAction;
        public Dictionary<MouseButton, PlayerActionEnum.PlayerActions> MouseAction;
    }
}
