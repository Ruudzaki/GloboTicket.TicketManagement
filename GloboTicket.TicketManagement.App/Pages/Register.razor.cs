using GloboTicket.TicketManagement.App.Contracts;
using GloboTicket.TicketManagement.App.ViewModels;
using Microsoft.AspNetCore.Components;

namespace GloboTicket.TicketManagement.App.Pages
{
    public partial class Register
    {

        public RegisterViewModel RegisterViewModel { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string Message { get; set; }

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; }

        public Register()
        {

        }
        protected override void OnInitialized()
        {
            RegisterViewModel = new RegisterViewModel();
        }

        protected async void HandleValidSubmit()
        {
            await AuthenticationService.Register(RegisterViewModel.Email, RegisterViewModel.Password);

            NavigationManager.NavigateTo("home");
        }
    }
}
