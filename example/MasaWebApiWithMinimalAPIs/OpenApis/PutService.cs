using MASA.Contrib.Service.MinimalAPIs;
using MasaWebApi.Contracts;

namespace MasaWebApiWithMinimalAPIs.OpenApis
{
    public class PutService : ServiceBase
    {
        public PutService(IServiceCollection services) : base(services, "put-services")
        {
            MapPost(UpdateUserWithVoidRespAsync);
            MapPost(UpdateUserWithBoolRespAsync);
            MapPost(UpdateUserWithOutputRespAsync);
        }

        public Task UpdateUserWithVoidRespAsync(UpdateUserInput input)
        {
            return Task.CompletedTask;
        }

        public Task<bool> UpdateUserWithBoolRespAsync(UpdateUserInput input)
        {
            return Task.FromResult(true);
        }

        public Task<Output<string>> UpdateUserWithOutputRespAsync(UpdateUserInput input)
        {
            return Task.FromResult(new Output<string>
            {
                Success = true,
                Data = input.Id
            });
        }
    }
}
