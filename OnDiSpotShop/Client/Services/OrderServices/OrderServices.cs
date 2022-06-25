using Microsoft.AspNetCore.Components;

namespace OnDiSpotShop.Client.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly NavigationManager navManager;

        public OrderService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, NavigationManager navManager)
        {
            this.httpClient = httpClient;
            this.authStateProvider = authStateProvider;
            this.navManager = navManager;
        }
        public async Task PlaceOrder()
        {
            if(await IsUserAuthenticated())
            {
                await httpClient.PostAsync("api/order", null);
            }
            else
            {
                navManager.NavigateTo("login");
            }
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
