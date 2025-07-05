using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Common;

namespace DataAcess
{
    public class ApplicationDbInitializer
    {

        public static async Task SeedUsersAndRole(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }

                //if (!await roleManager.RoleExistsAsync(UserRoles.User))
                //{
                //    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                //}


                //Users
                var UserManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var adminUser = await UserManager.FindByEmailAsync("admin@shinyspark009.com");
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        
                        UserName = "admin-user",
                        Email = "admin@shinyspark009.com",
                        EmailConfirmed = true
                    };
                    await UserManager.CreateAsync(newAdminUser, "Varsh@2024");
                    await UserManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                //var AppUser = await UserManager.FindByEmailAsync("AppUser");
                //if (AppUser == null)
                //{
                //    var newAppUser = new ApplicationUser()
                //    {
                        
                //        UserName = "app-user",
                //        Email = "AppUser",
                //        EmailConfirmed = true
                //    };
                //    await UserManager.CreateAsync(newAppUser, "Coding@1234");
                //    await UserManager.AddToRoleAsync(newAppUser, UserRoles.User);
                //}

            }
        }

    }
}
