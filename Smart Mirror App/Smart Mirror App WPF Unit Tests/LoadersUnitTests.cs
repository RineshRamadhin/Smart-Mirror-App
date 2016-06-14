using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using Smart_Mirror_App_WPF.Loaders;

namespace Smart_Mirror_App_WPF_Unit_Tests
{
    [TestClass]
    public class LoadersUnitTests
    {
        [TestMethod]
        public void XmlValidTest()
        {
            var xmlLoader = new XmlLoader();
            var xml = xmlLoader.Load("http://www.ad.nl/home/rss.xml");

            Assert.IsNotNull(xml);
        }

        [TestMethod]
        public void XmlInvalidTest()
        {
            var xmlLoader = new XmlLoader();
            var xml = xmlLoader.Load("http://www.ad.nl/home/");

            Assert.IsNull(xml);
        }

        [TestMethod]
        public void XmlInvalidTypeTest()
        {
            var xmlLoader = new XmlLoader();
            var xml = xmlLoader.Load("http://jsonplaceholder.typicode.com/posts");

            Assert.IsNull(xml);
        }
    }
}
