using Microsoft.AspNet.Identity.Owin;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Services;

namespace OutdoorSolution.Helpers
{
    public class IdentityHelper
    {
        public static void InitUserManager(TGUserManager userManager)
        {
            var dataProtectionProvider = Startup.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}