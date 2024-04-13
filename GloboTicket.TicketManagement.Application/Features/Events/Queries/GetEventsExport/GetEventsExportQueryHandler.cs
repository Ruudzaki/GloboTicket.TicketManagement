using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExport;

public class GetEventsExportQueryHandler : IRequestHandler<GetEventsExportQuery, EventsExportFileVm>
{
    private readonly ICsvExporter _csvExporter;
    private readonly IAsyncRepository<Event> _eventRepository;
    private readonly IMapper _mapper;

    public GetEventsExportQueryHandler(IAsyncRepository<Event> eventRepository, IMapper mapper,
        ICsvExporter csvExporter)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _csvExporter = csvExporter;
    }

    public async Task<EventsExportFileVm> Handle(GetEventsExportQuery request, CancellationToken cancellationToken)
    {
        var allEvents = _mapper.Map<List<EventExportDto>>((await _eventRepository
            .ListAllAsync()).OrderBy(x => x.Date));

        var fileData = _csvExporter.ExportEventsToCsv(allEvents);

        var eventExportFileDto = new EventsExportFileVm
            { ContentType = "text/csv", Data = fileData, EventExportFileName = $"{Guid.NewGuid()}.csv" };

        return eventExportFileDto;
    }
}