using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Orders.Queries.GetOrdersForMonth;

public class GetOrdersForMonthQueryHandler : IRequestHandler<GetOrdersForMonthQuery, PagedOrdersForMonthVm>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public GetOrdersForMonthQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<PagedOrdersForMonthVm> Handle(GetOrdersForMonthQuery request, CancellationToken cancellationToken)
    {
        var list = await _orderRepository.GetPagedOrdersForMonth(request.Date, request.Page, request.Size);
        var orders = _mapper.Map<List<OrdersForMonthDto>>(list);

        var count = await _orderRepository.GetTotalCountOfOrdersForMonth(request.Date);
        return new PagedOrdersForMonthVm
            { Count = count, OrdersForMonth = orders, Page = request.Page, Size = request.Size };
    }
}