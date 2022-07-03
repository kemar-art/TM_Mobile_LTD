namespace OnDiSpotShop.Server.Services.OrserServices
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder(int userId);
        Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders();
        Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId);
        //Task<ServiceResponse<bool>> AddOrderAsync(Order order);
    }
}
