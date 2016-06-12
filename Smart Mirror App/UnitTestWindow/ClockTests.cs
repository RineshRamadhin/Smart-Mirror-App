using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Smart_Mirror_App.Clock;

namespace UnitTestWindow
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cm = new ClockModel();
            cm.Update();
            var test = cm.CurrentTime;
        }
    }
}
