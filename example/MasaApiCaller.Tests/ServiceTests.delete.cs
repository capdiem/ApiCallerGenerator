using MasaWebApi.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MasaApiCaller.Tests
{
    public partial class ServiceTests
    {
        private readonly DeleteUserInput _userToDelete = new()
        {
            Id = "userid"
        };

        [TestMethod]
        public async Task DeleteUserWithObjectRequestAndOutputRespAsync()
        {
            var res = await _testCaller.DeleteService.DeleteUserWithObjectRequestAndOutputRespAsync(_userToDelete);

            Assert.IsTrue(res.Success);
            Assert.AreEqual("userid", res.Data);
        }
    }
}
