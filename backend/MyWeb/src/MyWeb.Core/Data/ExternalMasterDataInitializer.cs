using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using MyWeb.Core.Data.ExternalMasterData;

namespace MyWeb.Core.Data;

public static class MasterDataExternalInitializer
{
    public static void InitializeDevData(this IApplicationBuilder app,
        IConfiguration configuration)
    {
        IdentityData.InitializeData(app.ApplicationServices, configuration).Wait();
    }
}
