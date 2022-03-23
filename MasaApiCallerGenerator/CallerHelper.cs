using MasaApiCallerGenerator.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            return $@"// Auto-generated code
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
            var builder = new StringBuilder($@"// Auto-generated code
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

            builder.Append(@"    }
}");

            return builder.ToString();
        }

        static string GenerateService(ServiceModel service)
        {
            var builder = new StringBuilder($@"// Auto-generated code
using System.Collections.Generic;
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
                builder.Append($@"
        public {method.FormatedReturnType} {method.Name}({method.FormatedParameters})
        {{");
                builder.Append(GenMethodBody(method, service.BaseAdress));
                builder.Append(@"
        }
");
            }

            builder.Append(@"    }
}");

            return builder.ToString();
        }

        static string IsDaprCaller(bool isDapr)
        {
            return isDapr ? "DaprCallerBase" : "HttpClientCallerBase";
        }

        static string GenMethodBody(MethodModel method, string? baseAddress)
        {
            var sb = new StringBuilder();

            if (method.MethodInvoked == "Get")
            {
                bool queryExists = false;

                if (method.Query is Dictionary<string, string> queryDic)
                {
                    queryExists = true;

                    sb.Append(@"
            var query = new Dictionary<string, string>();");

                    foreach (var item in queryDic)
                    {
                        sb.Append($@"
            query[nameof({item.Key})] = {item.Key}.ToString();");
                    }

                    sb.AppendLine();
                }

                sb.Append($@"
            return CallerProvider.GetAsync<{method.ReturnType}>(""{method.GetFullUri(baseAddress)}""");

                if (queryExists)
                {
                    sb.Append(", query");
                }

                sb.Append(");");
            }
            else
            {
                string? requestVar = null;

                if (method.Query is Dictionary<string, string> queryDic)
                {
                    requestVar = queryDic.FirstOrDefault().Key;
                }

                sb.Append($@"
            return CallerProvider.{method.GenGenericMethod}(""{method.GetFullUri(baseAddress)}""");

                if (requestVar is not null)
                {
                    sb.Append($", {requestVar}");
                }

                sb.Append(");");
            }

            return sb.ToString();
        }
    }
}
