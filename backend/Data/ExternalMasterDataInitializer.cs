using Microsoft.AspNetCore.Builder;
using MyWeb.Data.ExternalMasterData;

namespace MyWeb.Data
{
    public static class MasterDataExternalInitializer
    {
        public static void InitializeData(this IApplicationBuilder app)
        {
            IdentityData.InitializeData(app.ApplicationServices).Wait();
        }
    }
}
