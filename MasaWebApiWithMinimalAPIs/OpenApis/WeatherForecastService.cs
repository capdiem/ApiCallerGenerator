using MASA.Contrib.Service.MinimalAPIs;

namespace MasaWebApiWithMinimalAPIs.OpenApis
{
    public class WeatherForecastService : ServiceBase
    {
        public WeatherForecastService(IServiceCollection services) : base(services, "weatherforecast")
        {
            MapGet(GetAsync);
            MapGet(GetString);
            MapGet(GetMulti);
        }

        public async Task<int> GetAsync(int num)
        {
            return num;
        }

        public string GetString(string str)
        {
            return str;
        }

        public string GetMulti(string firstName, string lastName)
        {
            return firstName + " " + lastName;
        }
    }
}
