using GloboTicket.TicketManagement.App.Auth;
using GloboTicket.TicketManagement.App.Contracts;
using GloboTicket.TicketManagement.App.Services.Base;

namespace GloboTicket.TicketManagement.App.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly CookieAuthenticationStateProvider _cookieAuthenticationStateProvider;

        public AuthenticationService(CookieAuthenticationStateProvider cookieAuthenticationStateProvider)
        {
            _cookieAuthenticationStateProvider = cookieAuthenticationStateProvider;
        }

        public async Task<ApiResponse> Login(string email, string password)
        {
            return await _cookieAuthenticationStateProvider.Login(email, password);
        }

        public  async Task<ApiResponse> Register(string email, string password)
        {
            return await _cookieAuthenticationStateProvider.Register(email, password);
        }

        public async Task Logout()
        {
            await _cookieAuthenticationStateProvider.Logout();
        }
    }
}