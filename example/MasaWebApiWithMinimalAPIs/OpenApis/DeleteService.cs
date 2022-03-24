using MASA.Contrib.Service.MinimalAPIs;
using MasaWebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MasaWebApiWithMinimalAPIs.OpenApis
{
    public class DeleteService : ServiceBase
    {
        public DeleteService(IServiceCollection services) : base(services, "delete-services")
        {
            MapDelete(DeleteUserWithObjectRequestAndOutputRespAsync);
        }

        // todo: masa caller not supported
        public Task DeleteUserWithVoidRespAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return Task.CompletedTask;
        }

        // todo: masa caller not supported
        public Task DeleteUserWithPlaceholdVarRequestAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return Task.CompletedTask;
        }

        // todo: masa caller not supported
        public Task<bool> DeleteUserWithBoolRespAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return Task.FromResult(true);
        }

        public Task<Output<string>> DeleteUserWithObjectRequestAndOutputRespAsync([FromBody]DeleteUserInput input)
        {
            return Task.FromResult(new Output<string>
            {
                Success = true,
                Data = input.Id
            });
        }
    }
}
