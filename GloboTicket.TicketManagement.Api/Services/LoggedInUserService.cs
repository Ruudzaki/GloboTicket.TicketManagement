using System.Security.Claims;
using GloboTicket.TicketManagement.Application.Contracts;

namespace GloboTicket.TicketManagement.Api.Services;

public class LoggedInUserService : ILoggedInUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                            throw new InvalidOperationException();
}