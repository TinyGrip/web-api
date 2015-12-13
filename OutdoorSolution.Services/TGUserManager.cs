using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Dal;

namespace OutdoorSolution.Services
{
    /// <summary>
    /// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    /// </summary>
    public class TGUserManager : UserManager<ApplicationUser>
    {
        private static IUserStore<ApplicationUser> GetUserStore(ApplicationDbContext dbContext)
        {
            return new UserStore<ApplicationUser>(dbContext);
        }

        public TGUserManager(ApplicationDbContext dbContext)
            : base(GetUserStore(dbContext))
        {
            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = true
                //RequireLowercase = true,
                //RequireUppercase = true,
            };
        }
    }
}
