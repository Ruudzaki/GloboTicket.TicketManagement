namespace GloboTicket.TicketManagement.Application.Features.Events;

public class CategoryDto
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
}