// Auto-generated code
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

        private DeleteService _deleteService;
        public DeleteService DeleteService => _deleteService ??= new DeleteService(CallerProvider);

        private GetService _getService;
        public GetService GetService => _getService ??= new GetService(CallerProvider);

        private PostService _postService;
        public PostService PostService => _postService ??= new PostService(CallerProvider);

        private PutService _putService;
        public PutService PutService => _putService ??= new PutService(CallerProvider);
    }
}