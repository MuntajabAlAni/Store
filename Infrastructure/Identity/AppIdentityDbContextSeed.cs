using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppIdentityDbContextSeed
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var user = new AppUser
            {
                DisplayName = "Jojo",
                Email = "jojo@test.com",
                UserName = "jojo@test.com",
                Address = new Address
                {
                    FirstName = "Jojo",
                    LastName = "AlAni",
                    Street = "Mansoor",
                    City = "Baghdad",
                    State = "BGW",
                    ZipCode = "10001"
                }
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
        }
    }
}