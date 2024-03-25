namespace GloboTicket.TicketManagement.Application.Features.Events;

public class EventDetailVm
{
    public Guid EventId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Price { get; set; } = string.Empty;

    public string Artist { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public CategoryDto Category { get; set; }
}