using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Models;

namespace Server.Pages.ShinySparkle
{
    public partial class Login
    {
        private LoginVM loginModel = new();
        private string errorMessage = string.Empty;

       
        private async Task HandleLogin()
        {
            var success = await AuthService.Login(loginModel);

            if (!success)
            {
                errorMessage = "Invalid login credentials. Please try again.";
            }
        

        }
    }
}