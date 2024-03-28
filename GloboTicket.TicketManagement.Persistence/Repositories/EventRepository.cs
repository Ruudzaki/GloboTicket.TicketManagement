using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Persistence.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(GloboTicketDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate)
    {
        var matches = _dbContext.Events.AnyAsync(e => e.Name.Equals(name) &&
                                                      e.Date.Date.Equals(eventDate.Date));
        return matches;
    }
}