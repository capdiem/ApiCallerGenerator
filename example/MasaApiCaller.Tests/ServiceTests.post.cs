using MasaWebApi.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MasaApiCaller.Tests
{
    public partial class ServiceTests
    {
        private readonly UpdateUserInput _userToCreate = new()
        {
            Id = "userid",
            Name = "capdiem"
        };

        [TestMethod]
        public async Task UpdateUserWithVoidRespAsync()
        {
            await _testCaller.PutService.UpdateUserWithVoidRespAsync(_userToCreate);
        }

        [TestMethod]
        public async Task UpdateUserWithBoolRespAsync()
        {
            var res = await _testCaller.PutService.UpdateUserWithBoolRespAsync(_userToCreate);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public async Task UpdateUserWithOutputRespAsync()
        {
            var res = await _testCaller.PutService.UpdateUserWithOutputRespAsync(_userToCreate);

            Assert.IsTrue(res.Success);
            Assert.AreEqual("userid", res.Data);
        }
    }
}
