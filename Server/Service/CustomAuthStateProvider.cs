using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace Server.Service
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new ClaimsIdentity();

            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");

                if (!string.IsNullOrWhiteSpace(token))
                {
                    var claims = ParseClaimsFromJwt(token);

                    // Optional: Check expiration claim here if you're storing "exp"
                    identity = new ClaimsIdentity(claims, "jwt");
                }
            }
            catch
            {
                // fail safe
                await _localStorage.RemoveItemAsync("authToken");
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }


        public async Task SetToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                await _localStorage.RemoveItemAsync("authToken");
            }
            else
            {
                await _localStorage.SetItemAsync("authToken", token);
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("JWT token is null or empty");
            }

            try
            {
                var payload = token.Split('.')[1]; // Get payload part
                var jsonBytes = Convert.FromBase64String(PadBase64String(payload)); // Ensure valid base64
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
            }
            catch (FormatException ex)
            {
                throw new FormatException("Invalid Base64 JWT token format", ex);
            }
        }


        private string PadBase64String(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: return base64 + "==";
                case 3: return base64 + "=";
                default: return base64;
            }
        }

    }
}

