// Auto-generated code
using System.Collections.Generic;
using System.Threading.Tasks;
using Masa.Utils.Caller.Core;

namespace MasaApiCaller
{
    public partial class PutService : ServiceBase
    {
        public PutService(ICallerProvider callerProvider) : base(callerProvider) {}

        public Task UpdateUserWithVoidRespAsync(MasaWebApi.Contracts.UpdateUserInput input)
        {
            return CallerProvider.PostAsync<MasaWebApi.Contracts.UpdateUserInput>("put-services/UpdateUserWithVoidResp", input);
        }

        public Task<bool> UpdateUserWithBoolRespAsync(MasaWebApi.Contracts.UpdateUserInput input)
        {
            return CallerProvider.PostAsync<MasaWebApi.Contracts.UpdateUserInput, bool>("put-services/UpdateUserWithBoolResp", input);
        }

        public Task<MasaWebApi.Contracts.Output<string>> UpdateUserWithOutputRespAsync(MasaWebApi.Contracts.UpdateUserInput input)
        {
            return CallerProvider.PostAsync<MasaWebApi.Contracts.UpdateUserInput, MasaWebApi.Contracts.Output<string>>("put-services/UpdateUserWithOutputResp", input);
        }
    }
}