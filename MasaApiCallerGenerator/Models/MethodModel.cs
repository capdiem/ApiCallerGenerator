using Microsoft.CodeAnalysis;
using System.Collections.Generic;
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

        public string MethodOfICallerProvider => $"{MethodInvoked}Async";

        public string FormatedReturnType => ReturnType == "void" ? "Task" : $"Task<{ReturnType}>";
    }
}
