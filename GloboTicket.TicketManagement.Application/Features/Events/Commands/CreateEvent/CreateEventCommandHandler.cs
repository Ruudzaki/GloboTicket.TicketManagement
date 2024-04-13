using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Exceptions;
using GloboTicket.TicketManagement.Application.Models.Mail;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
{
    private readonly IEmailService _emailService;
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public CreateEventCommandHandler(IMapper mapper, IEventRepository eventRepository, IEmailService emailService)
    {
        _mapper = mapper;
        _eventRepository = eventRepository;
        _emailService = emailService;
    }

    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateEventCommandValidator(_eventRepository);
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (validatorResult.Errors.Count > 0)
            throw new ValidationException(validatorResult);

        var @event = _mapper.Map<Event>(request);

        @event = await _eventRepository.AddAsync(@event);

        var email = new Email
        {
            To = "kostybelous@gmail.com",
            Body = $"A new event was created: {request}",
            Subject = "A new event was created"
        };
        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception e)
        {
            //this shouldn't stop the API from doing other things
        }

        return @event.EventId;
    }
}