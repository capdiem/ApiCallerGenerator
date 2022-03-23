using MASA.Contrib.Service.MinimalAPIs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;

namespace MasaWebApiWithMinimalAPIs.OpenApis
{
    public class GetService : ServiceBase
    {
        public GetService(IServiceCollection services) : base(services, "get-services")
        {
            //MapGet(GetWithEmptyParamsAsync);
            //MapGet(GetWithSingleParamAsync);
            //MapGet(GetWithTwoParamsAsync);
            //MapGet(GetWithCustomUriAsync, customUri: "custom-uri");
            //MapGet(GetWithAsyncSuffixAsync, trimEndAsync: false);

            App.MapGet("empty", GetWithEmptyParamsAsync);
            App.MapGet("single",GetWithSingleParamAsync);
            App.MapGet("two",GetWithTwoParamsAsync);
            App.MapGet("custom-uri",GetWithCustomUriAsync);
            App.MapGet("async-suffix", GetWithAsyncSuffixAsync);
        }

        public Task<string> GetWithEmptyParamsAsync([FromServices] ISwaggerProvider swaggerProvider)
        {
            return Task.FromResult("GetWithEmptyParamsAsync");
        }

        public Task<int> GetWithSingleParamAsync([FromServices] ISwaggerProvider swaggerProvider, int num)
        {
            return Task.FromResult(num);
        }

        public Task<string> GetWithTwoParamsAsync(string firstName, [FromServices] ISwaggerProvider swaggerProvider, [FromQuery] string lastName)
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
