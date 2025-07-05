using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Models;

namespace Server.Service
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly CustomAuthStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        public AuthService(HttpClient http, ILocalStorageService localStorage, CustomAuthStateProvider authStateProvider, NavigationManager navigationManager)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<bool> Login(LoginVM loginModel)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (!string.IsNullOrEmpty(result?.Token))
                {
                    //await _localStorage.SetItemAsync("authToken", result.Token);
                    await _authStateProvider.SetToken(result.Token);
                    _navigationManager.NavigateTo("/ProductList", forceLoad: true);
                    return true;
                }
            }

            return false;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _authStateProvider.SetToken(null);
            _navigationManager.NavigateTo("/login", forceLoad: true);
        }
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
    }


}

