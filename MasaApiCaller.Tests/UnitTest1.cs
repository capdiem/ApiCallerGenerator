using Masa.Utils.Caller.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MasaApiCaller.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var services = new ServiceCollection();
            services.AddCaller(typeof(TestCaller).Assembly);

            var provider = services.BuildServiceProvider();

            var caller = provider.GetService<TestCaller>();
            var res = await caller.WeatherForecastService.GetAsync(1);

            Assert.AreEqual(0, res);
        }
    }
}