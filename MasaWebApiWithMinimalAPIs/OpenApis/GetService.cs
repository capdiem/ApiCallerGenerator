using MASA.Contrib.Service.MinimalAPIs;

namespace MasaWebApiWithMinimalAPIs.OpenApis
{
    public class GetService : ServiceBase
    {
        public GetService(IServiceCollection services) : base(services, "get-services")
        {
            MapGet(GetWithEmptyParamsAsync);
            MapGet(GetWithSingleParamAsync);
            MapGet(GetWithTwoParamsAsync);
            MapGet(GetWithCustomUriAsync, customUri: "custom-uri");
            MapGet(GetWithAsyncSuffixAsync, trimEndAsync: false);
        }

        public Task<string> GetWithEmptyParamsAsync()
        {
            return Task.FromResult("GetWithEmptyParamsAsync");
        }

        public Task<int> GetWithSingleParamAsync(int num)
        {
            return Task.FromResult(num);
        }

        public Task<string> GetWithTwoParamsAsync(string firstName, string lastName)
        {
            return Task.FromResult(firstName + " " + lastName);
        }

        public Task<string> GetWithCustomUriAsync()
        {
            return Task.FromResult("GetWithCustomUriAsync");
        }

        public Task<string> GetWithAsyncSuffixAsync()
        {
            return Task.FromResult("GetWithAsyncSuffixAsync");
        }
    }
}
