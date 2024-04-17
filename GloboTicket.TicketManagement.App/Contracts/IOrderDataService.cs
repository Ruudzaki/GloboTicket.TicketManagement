using GloboTicket.TicketManagement.App.ViewModels;

namespace GloboTicket.TicketManagement.App.Contracts
{
    public interface IOrderDataService
    {
        Task<PagedOrderForMonthViewModel> GetPagedOrderForMonth(DateTime date, int page, int size);
    }
}
