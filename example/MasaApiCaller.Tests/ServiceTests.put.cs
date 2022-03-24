using MasaWebApi.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MasaApiCaller.Tests
{
    public partial class ServiceTests
    {
        private readonly CreateUserInput _userToUpdate = new()
        {
            Name = "capdiem"
        };

        [TestMethod]
        public async Task CreateUserWithVoidRespAsync()
        {
            await _testCaller.PostService.CreateUserWithVoidRespAsync(_userToUpdate);
        }

        [TestMethod]
        public async Task CreateUserWithBoolRespAsync()
        {
            var res = await _testCaller.PostService.CreateUserWithBoolRespAsync(_userToUpdate);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task CreateUserWithOutputRespAsync()
        {
            var res = await _testCaller.PostService.CreateUserWithOutputRespAsync(_userToUpdate);

            Assert.IsTrue(res.Success);
            Assert.AreEqual("userid", res.Data);
        }
    }
}
