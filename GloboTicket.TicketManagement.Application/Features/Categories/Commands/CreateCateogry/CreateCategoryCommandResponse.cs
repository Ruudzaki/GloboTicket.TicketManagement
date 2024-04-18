using GloboTicket.TicketManagement.Application.Responses;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry;

public class CreateCategoryCommandResponse : BaseResponse
{
    public CreateCategoryDto Category { get; set; } = default!;
}