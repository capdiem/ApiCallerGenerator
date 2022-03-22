using System.Collections.Generic;
using System.Threading.Tasks;
using Masa.Utils.Caller.Core;

namespace MasaApiCaller
{
    public partial class PostService : ServiceBase
    {
        public PostService(ICallerProvider callerProvider) : base(callerProvider) {}

        public Task CreateUserWithVoidRespAsync(MasaWebApi.Contracts.CreateUserInput input)
        {
            return CallerProvider.PostAsync<MasaWebApi.Contracts.CreateUserInput>("post-services/CreateUserWithVoidResp", input);
        }

        public Task<bool> CreateUserWithBoolRespAsync(MasaWebApi.Contracts.CreateUserInput input)
        {
            return CallerProvider.PostAsync<MasaWebApi.Contracts.CreateUserInput, bool>("post-services/CreateUserWithBoolResp", input);
        }

        public Task<MasaWebApi.Contracts.Output<string>> CreateUserWithOutputRespAsync(MasaWebApi.Contracts.CreateUserInput input)
        {
            return CallerProvider.PostAsync<MasaWebApi.Contracts.CreateUserInput, MasaWebApi.Contracts.Output<string>>("post-services/CreateUserWithOutputResp", input);
        }

    }
}