using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasaApiCallerGenerator.Models
{
    internal class MethodModel
    {
        public string? Name { get; set; }

        public object? Query { get; set; }

        public string? RelativeUri { get; set; }

        public string? ReturnType { get; set; }

        public string MethodInvoked { get; set; } = null!;

        public string FormatedReturnType => ReturnType == "void" ? "Task" : $"Task<{ReturnType}>";

        public string MethodOfICallerProvider => $"{MethodInvoked}Async";

        public bool IsOriginAppMap { get; set; }

        public string GenGenericMethod
        {
            get
            {
                if (MethodInvoked == "Get")
                {
                    return $"GetAsync<{ReturnType}>";
                }

                if (ReturnType == "void")
                {
                    return $"{MethodInvoked}Async<{RequestType}>";
                }

                return $"{MethodInvoked}Async<{RequestType}, {ReturnType}>";

            }
        }

        public string FormatedParameters
        {
            get
            {
                if (Query is null)
                {
                    return string.Empty;
                }

                var p = new StringBuilder();

                if (Query is Dictionary<string, string> dicQuery)
                {
                    foreach (var kv in dicQuery)
                    {
                        p.Append($"{kv.Value} {kv.Key}, ");
                    }

                    var str = p.ToString();
                    return str.Substring(0, str.Length - 2);
                }

                return string.Empty;
            }
        }

        public string? RequestType
        {
            get
            {
                if (Query is null)
                {
                    return null;
                }

                if (Query is Dictionary<string, string> dicQuery)
                {
                    return dicQuery.FirstOrDefault().Value;
                }

                return null;
            }
        }

        public string? GetFullUri(string? baseAddress)
        {
            return IsOriginAppMap || baseAddress is null
                ? RelativeUri
                : $"{baseAddress}/{RelativeUri}";
        }
    }
}
