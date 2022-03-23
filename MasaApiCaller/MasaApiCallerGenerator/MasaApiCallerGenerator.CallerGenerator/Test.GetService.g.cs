// Auto-generated code
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
            return CallerProvider.GetAsync<string>("empty");
        }

        public Task<int> GetWithSingleParamAsync(int num)
        {
            var query = new Dictionary<string, string>();
            query[nameof(num)] = num.ToString();

            return CallerProvider.GetAsync<int>("single", query);
        }

        public Task<string> GetWithTwoParamsAsync(string firstName, string lastName)
        {
            var query = new Dictionary<string, string>();
            query[nameof(firstName)] = firstName.ToString();
            query[nameof(lastName)] = lastName.ToString();

            return CallerProvider.GetAsync<string>("two", query);
        }

        public Task<string> GetWithCustomUriAsync()
        {
            return CallerProvider.GetAsync<string>("custom-uri");
        }

        public Task<string> GetWithAsyncSuffixAsync()
        {
            return CallerProvider.GetAsync<string>("async-suffix");
        }
    }
}