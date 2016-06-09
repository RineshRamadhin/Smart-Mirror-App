using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart_Mirror_App_WPF.Data.API;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF_Unit_Tests
{
    [TestClass]
    public class OpenWeatherApiUnitTests
    {
        [TestMethod]
        public async Task OpenWeatherServiceTest()
        {
            var openWeatherService = new OpenWeatherService();
            var currentWeather = openWeatherService.GetData();
            Assert.IsNotNull(currentWeather);
        }
    }
}
