using MasaApiCallerGenerator.Models;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MasaApiCallerGenerator
{
    internal static class CallerHelper
    {
        const string Namespace = "MasaApiCaller";

        internal static List<(string hintName, string sourceText)> GenerateSources(CallerModel caller)
        {
            var serviceBaseSource = ("ServiceBase.g.cs", GenerateServiceBase());
            var callerSource = ($"{caller.FullName}.g.cs", GenerateCaller(caller));

            var sources = new List<(string hintName, string sourceText)>()
            {
                serviceBaseSource,
                callerSource,
            };

            foreach (var service in caller.Services)
            {
                var serviceSource = GenerateService(service);

                sources.Add(($"{caller.Name}.{service.Name}.g.cs", serviceSource));
            }

            return sources;
        }

        static string GenerateServiceBase()
        {
            return $@"
using Masa.Utils.Caller.Core;

namespace {Namespace}
{{
    public abstract partial class ServiceBase
    {{
        protected ICallerProvider CallerProvider {{ get; init; }}

        protected ServiceBase(ICallerProvider callerProvider)
        {{
            CallerProvider = callerProvider;
        }}
    }}
}}
";
        }

        static string GenerateCaller(CallerModel caller)
        {
            var builder = new StringBuilder($@"
using System;
using Masa.Utils.Caller.HttpClient;

namespace {Namespace}
{{
    public partial class {caller.FullName} : HttpClientCallerBase
    {{
        protected override string BaseAddress {{ get; set; }}
        
        public {caller.FullName}(IServiceProvider serviceProvider): base(serviceProvider)
        {{
            Name = nameof({caller.FullName});
            BaseAddress = ""{caller.BaseAddress}"";
        }}
");

            foreach (var service in caller.Services)
            {
                builder.Append($@"
        private {service.Name} {service.PrivateField};
        public {service.Name} {service.Name} => {service.PrivateField} ??= new {service.Name}(CallerProvider);
");
            }

            builder.Append(@"
    }
}");

            return builder.ToString();
        }

        static string GenerateService(ServiceModel service)
        {
            var builder = new StringBuilder($@"
using System.Threading.Tasks;
using Masa.Utils.Caller.Core;

namespace {Namespace}
{{
    public partial class {service.Name} : ServiceBase
    {{
        public {service.Name}(ICallerProvider callerProvider) : base(callerProvider) {{}}
");

            foreach (var method in service.Methods)
            {
                var parameters = FormatedParameters(method.Query);

                builder.Append($@"
        public {method.FormatedReturnType} {method.Name}({parameters})
        {{
");
                builder.Append($@"
            return CallerProvider.{method.MethodOfICallerProvider}<{method.ReturnType}>(""{method.RelativeUri}"");
        }}
");
            }

            builder.Append(@"
    }
}");

            return builder.ToString();
        }

        static string IsDaprCaller(bool isDapr)
        {
            return isDapr ? "DaprCallerBase" : "HttpClientCallerBase";
        }

        static string FormatedParameters(object? query)
        {
            if (query is null)
            {
                return string.Empty;
            }

            var p = new StringBuilder();

            if (query is Dictionary<string, string> dicQuery)
            {
                foreach (var kv in dicQuery)
                {
                    p.Append($"{kv.Value} {kv.Key}, ");
                }

                var str = p.ToString();
                return str.Substring(0, str.Length - 3);
            }

            return string.Empty;
        }
    }
}
