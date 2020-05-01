using ComputergrafikSpiel.Controller;
using ComputergrafikSpiel.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Input;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Test.Controller
{
    [TestClass]
    public class InputControllerTest
    {
        [TestMethod]
        public void InputControllerConstructorTest()
        {
            InputControllerSettings controllerSettings = new InputControllerSettings();
            InputController controller = new InputController(controllerSettings);
        }
    }
}
