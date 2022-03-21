
using System;
using Masa.Utils.Caller.HttpClient;

namespace MasaApiCaller
{
    public partial class TestCaller : HttpClientCallerBase
    {
        protected override string BaseAddress { get; set; }
        
        public TestCaller(IServiceProvider serviceProvider): base(serviceProvider)
        {
            Name = nameof(TestCaller);
            BaseAddress = "http://localhost:5177";
        }

        private WeatherForecastService _WeatherForecastService;
        public WeatherForecastService WeatherForecastService => _WeatherForecastService ??= new WeatherForecastService(CallerProvider);

    }
}