using MasaWebApi.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MasaApiCaller.Tests
{
    public partial class ServiceTests
    {
        private readonly CreateUserInput _testUser = new CreateUserInput()
        {
            Id = "userid",
            Name = "capdiem"
        };

        [TestMethod]
        public async Task CreateUserWithVoidRespAsync()
        {
            await _testCaller.PostService.CreateUserWithVoidRespAsync(_testUser);
        }

        [TestMethod]
        public async Task CreateUserWithBoolRespAsync()
        {
            var res = await _testCaller.PostService.CreateUserWithBoolRespAsync(_testUser);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task CreateUserWithOutputRespAsync()
        {
            var res = await _testCaller.PostService.CreateUserWithOutputRespAsync(_testUser);

            Assert.IsTrue(res.Success);
            Assert.AreEqual(_testUser.Id, res.Data);
        }
    }
}
