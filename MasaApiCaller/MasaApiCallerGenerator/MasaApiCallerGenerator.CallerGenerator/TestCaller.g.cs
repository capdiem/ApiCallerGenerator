
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

        private GetService _GetService;
        public GetService GetService => _GetService ??= new GetService(CallerProvider);

        private PostService _PostService;
        public PostService PostService => _PostService ??= new PostService(CallerProvider);

    }
}