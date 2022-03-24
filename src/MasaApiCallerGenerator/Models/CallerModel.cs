using System;
using System.Collections.Generic;
using System.Text;

namespace MasaApiCallerGenerator.Models
{
    internal class CallerModel
    {
        public string? Name { get; set; }

        public string? BaseAddress { get; set; }

        public List<ServiceModel> Services { get; set; } = new();

        public string FullName => Name + "Caller";
    }
}
