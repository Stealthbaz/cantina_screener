using System;
using cantinaScreenerConsole;
using cantinaScreenerConsole.InputProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace screenerTestProject1
{
    [TestClass]
    public class MainProcessorTest
    {
        [TestMethod]
        public void CanExitApplicationTest()
        {
            Controller c = new Controller();
            cantinaScreenerConsole.Model.JSONModel j = new cantinaScreenerConsole.Model.JSONModel();
            MainMenuProcessor mainMenuProcessor = new MainMenuProcessor(c, j);
            string input = "x";
            bool expectedResult = true;
            bool shouldExitNow = ((IInputProcessor)mainMenuProcessor).Process(input);
            Assert.AreEqual(expectedResult, shouldExitNow);

            input = "anythingButX";
            expectedResult = false;
            shouldExitNow = ((IInputProcessor)mainMenuProcessor).Process(input);
            Assert.AreEqual(expectedResult, shouldExitNow);
        }
    }
}
