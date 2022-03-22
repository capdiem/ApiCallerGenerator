using Masa.Utils.Caller.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MasaApiCaller.Tests
{
    [TestClass]
    public partial class ServiceTests
    {
        private readonly TestCaller _testCaller;

        public ServiceTests()
        {
            var services = new ServiceCollection();
            services.AddCaller(typeof(TestCaller).Assembly);

            var provider = services.BuildServiceProvider();

            _testCaller = provider.GetService<TestCaller>();
        }

        [TestMethod]
        public async Task GetWithEmptyParamsAsync()
        {
            var res = await _testCaller.GetService.GetWithEmptyParamsAsync();

            Assert.AreEqual("GetWithEmptyParamsAsync", res);
        }


        [TestMethod]
        public async Task GetWithSingleParamAsync()
        {
            var num = 110;

            var res = await _testCaller.GetService.GetWithSingleParamAsync(num);

            Assert.AreEqual(num, res);
        }


        [TestMethod]
        public async Task GetWithTwoParamsAsync()
        {
            string name = "Capdiem Cao";
            var names = name.Split(" ");

            var res = await _testCaller.GetService.GetWithTwoParamsAsync(names[0], names[1]);

            Assert.AreEqual(name, res);
        }


        [TestMethod]
        public async Task GetWithCustomUriAsync()
        {
            var res = await _testCaller.GetService.GetWithCustomUriAsync();

            Assert.AreEqual("GetWithCustomUriAsync", res);
        }


        [TestMethod]
        public async Task GetWithAsyncSuffixAsync()
        {
            var res = await _testCaller.GetService.GetWithAsyncSuffixAsync();

            Assert.AreEqual("GetWithAsyncSuffixAsync", res);
        }
    }
}