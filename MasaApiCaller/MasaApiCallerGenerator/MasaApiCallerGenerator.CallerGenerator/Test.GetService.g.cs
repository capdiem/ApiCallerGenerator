using System.Collections.Generic;
using System.Threading.Tasks;
using Masa.Utils.Caller.Core;

namespace MasaApiCaller
{
    public partial class GetService : ServiceBase
    {
        public GetService(ICallerProvider callerProvider) : base(callerProvider) {}

        public Task<string> GetWithEmptyParamsAsync()
        {
            return CallerProvider.GetAsync<string>("get-services/GetWithEmptyParams");
        }

        public Task<int> GetWithSingleParamAsync(int num)
        {
            var query = new Dictionary<string, string>();
            query[nameof(num)] = num.ToString();

            return CallerProvider.GetAsync<int>("get-services/GetWithSingleParam", query);
        }

        public Task<string> GetWithTwoParamsAsync(string firstName, string lastName)
        {
            var query = new Dictionary<string, string>();
            query[nameof(firstName)] = firstName.ToString();
            query[nameof(lastName)] = lastName.ToString();

            return CallerProvider.GetAsync<string>("get-services/GetWithTwoParams", query);
        }

        public Task<string> GetWithCustomUriAsync()
        {
            return CallerProvider.GetAsync<string>("get-services/custom-uri");
        }

        public Task<string> GetWithAsyncSuffixAsync()
        {
            return CallerProvider.GetAsync<string>("get-services/GetWithAsyncSuffixAsync");
        }

    }
}