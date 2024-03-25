using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events;

public class GetEventsListQuery : IRequest<List<EventListVm>>
{
}