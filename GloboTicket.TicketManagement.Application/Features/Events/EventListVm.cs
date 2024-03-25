﻿namespace GloboTicket.TicketManagement.Application.Features.Events;

public class EventListVm
{
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}