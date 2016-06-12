using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Smart_Mirror_App.Clock;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTestClass
    {
        [TestMethod]
        public void TestTime()
        {
            var cm = new ClockModel();
            cm.Update();
            var cvm = new ClockViewModel();
            cvm.Initialize(cm);
            var cmTime = cm.CurrentTime;
            var cvmTime = cvm.CurrentTime;
           Assert.AreEqual("16:03", cvmTime);


        }
    }
}
