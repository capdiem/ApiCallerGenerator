
using System.Threading.Tasks;
using Masa.Utils.Caller.Core;

namespace MasaApiCaller
{
    public partial class WeatherForecastService : ServiceBase
    {
        public WeatherForecastService(ICallerProvider callerProvider) : base(callerProvider) {}

        public Task<int> GetAsync(int nu)
        {

            return CallerProvider.GetAsync<int>("Get");
        }

        public Task<string> GetString(string st)
        {

            return CallerProvider.GetAsync<string>("GetString");
        }

        public Task<string> GetMulti(string firstName, string lastNam)
        {

            return CallerProvider.GetAsync<string>("GetMulti");
        }

    }
}