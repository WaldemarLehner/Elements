using ComputergrafikSpiel.Controller.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComputergrafikSpiel.Test.Controller.Input
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
