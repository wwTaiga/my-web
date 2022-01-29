using Microsoft.AspNetCore.Builder;
using MyWeb.Core.Data.ExternalMasterData;

namespace MyWeb.Core.Data
{
    public static class MasterDataExternalInitializer
    {
        public static void InitializeDevData(this IApplicationBuilder app)
        {
            IdentityData.InitializeData(app.ApplicationServices).Wait();
        }
    }
}
