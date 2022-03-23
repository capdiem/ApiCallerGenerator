// Auto-generated code
using System.Collections.Generic;
using System.Threading.Tasks;
using Masa.Utils.Caller.Core;

namespace MasaApiCaller
{
    public partial class DeleteService : ServiceBase
    {
        public DeleteService(ICallerProvider callerProvider) : base(callerProvider) {}

        public Task<MasaWebApi.Contracts.Output<string>> DeleteUserWithObjectRequestAndOutputRespAsync(MasaWebApi.Contracts.DeleteUserInput input)
        {
            return CallerProvider.DeleteAsync<MasaWebApi.Contracts.DeleteUserInput, MasaWebApi.Contracts.Output<string>>("delete-services/DeleteUserWithObjectRequestAndOutputResp", input);
        }
    }
}