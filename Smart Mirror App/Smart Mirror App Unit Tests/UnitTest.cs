﻿using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using Smart_Mirror_App.Authentication;
namespace Smart_Mirror_App_Unit_Tests
{
    [TestClass]
    public class UnitTestsAuthentication
    {
        [TestMethod]
        public void GetToken_Test()
        {
            var googleAuthentication = new GoogleAuthentication();
            //var token = googleAuthentication.GetToken();
            Assert.AreEqual("", "");
        }
    }
}