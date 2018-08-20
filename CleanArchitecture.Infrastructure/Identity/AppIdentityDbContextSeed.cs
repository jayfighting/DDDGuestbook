using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "test@loandepot.com", Email = "test@loandepot.com" };
            await userManager.CreateAsync(defaultUser, "Testing@123");
        }
    }
}
