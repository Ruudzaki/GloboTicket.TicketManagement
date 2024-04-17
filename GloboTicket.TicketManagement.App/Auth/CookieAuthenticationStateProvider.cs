using GloboTicket.TicketManagement.App.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace GloboTicket.TicketManagement.App.Auth
{
    public class CookieAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        private bool _authenticated = false;

        private readonly ClaimsPrincipal Unauthenticated = new(new ClaimsIdentity());

        public CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Authentication");
        }

        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async Task<ApiResponse> Login(string email, string password)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync(
                    "login?useCookies=true", new
                    {
                        email,
                        password
                    });

                if (result.IsSuccessStatusCode)
                {
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

                    return new ApiResponse { Success = true };
                }
            }
            catch
            {
            }

            return new ApiResponse
            {
                Success = false,
                ValidationErrors = "Invalid email and/or password."
            };
        }

        public async Task<ApiResponse> Register(string email, string password)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync(
                    "register", new
                    {
                        email,
                        password
                    });

                if (result.IsSuccessStatusCode)
                {
                    return new ApiResponse { Success = true };
                }

                var details = await result.Content.ReadAsStringAsync();
                var problemDetails = JsonDocument.Parse(details);
                string errors = string.Empty;
                var errorList = problemDetails.RootElement.GetProperty("errors");

                foreach (var errorEntry in errorList.EnumerateObject())
                {
                    errors += errorEntry.Value.GetString()!;
                }

                return new ApiResponse
                {
                    Success = false,
                    ValidationErrors = errors
                };
            }
            catch { }

            return new ApiResponse
            {
                Success = false,
                ValidationErrors = "An unknown error occured, please try again."
            };
        }

        public async Task Logout()
        {
            await _httpClient.PostAsync("Logout", null);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _authenticated = false;

            var user = Unauthenticated;

            try
            {
                var userResponse = await _httpClient.GetAsync("manage/info");

                userResponse.EnsureSuccessStatusCode();

                var userJson = await userResponse.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson, jsonSerializerOptions);

                if (userInfo != null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, userInfo.Email),
                        new(ClaimTypes.Email, userInfo.Email)
                    };

                    claims.AddRange(
                        userInfo.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
                            .Select(c => new Claim(c.Key, c.Value)));

                    var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
                    user = new ClaimsPrincipal(id);
                    _authenticated = true;
                }
            }
            catch { }

            return new AuthenticationState(user);
        }
    }
}
