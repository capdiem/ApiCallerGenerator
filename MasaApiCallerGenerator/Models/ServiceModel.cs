using System.Collections.Generic;

namespace MasaApiCallerGenerator.Models
{
    internal class ServiceModel
    {
        public string? Name { get; set; }

        public string? BaseAdress { get; set; }

        public List<MethodModel> Methods { get; set; } = new();

        public string PrivateField => $"_{Name}";
    }
}
