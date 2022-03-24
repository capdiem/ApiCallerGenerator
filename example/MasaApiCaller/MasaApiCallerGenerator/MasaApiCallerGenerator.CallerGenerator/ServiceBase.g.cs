// Auto-generated code
using Masa.Utils.Caller.Core;

namespace MasaApiCaller
{
    public abstract partial class ServiceBase
    {
        protected ICallerProvider CallerProvider { get; init; }

        protected ServiceBase(ICallerProvider callerProvider)
        {
            CallerProvider = callerProvider;
        }
    }
}
