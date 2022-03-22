using MASA.Contrib.Service.MinimalAPIs;
using MasaWebApi.Contracts;

namespace MasaWebApiWithMinimalAPIs.OpenApis
{
    public class PostService : ServiceBase
    {
        public PostService(IServiceCollection services) : base(services, "post-services")
        {
            MapPost(CreateUserWithVoidRespAsync);
            MapPost(CreateUserWithBoolRespAsync);
            MapPost(CreateUserWithOutputRespAsync);
        }

        public Task CreateUserWithVoidRespAsync(CreateUserInput input)
        {
            return Task.CompletedTask;
        }

        public Task<bool> CreateUserWithBoolRespAsync(CreateUserInput input)
        {
            return Task.FromResult(true);
        }

        public Task<Output<string>> CreateUserWithOutputRespAsync(CreateUserInput input)
        {
            return Task.FromResult(new Output<string>
            {
                Success = true,
                Data = input.Id
            });
        }
    }
}
