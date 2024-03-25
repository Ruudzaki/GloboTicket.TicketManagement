using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventList;

public class GetEventsListQuery : IRequest<List<EventListVm>>
{
}