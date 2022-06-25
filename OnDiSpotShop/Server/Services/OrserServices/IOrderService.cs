namespace OnDiSpotShop.Server.Services.OrserServices
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();
        //Task<ServiceResponse<bool>> AddOrderAsync(Order order);
    }
}
