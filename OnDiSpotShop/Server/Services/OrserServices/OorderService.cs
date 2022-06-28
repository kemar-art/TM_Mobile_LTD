using System.Security.Claims;

namespace OnDiSpotShop.Server.Services.OrserServices
{
    public class OrderService : IOrderService
    {
        private readonly DataContext context;
        private readonly ICartService cartService;
        private readonly IAuthService authService;

        public OrderService(DataContext context, ICartService cartService, IAuthService authService)
        {
            this.context = context;
            this.cartService = cartService;
            this.authService = authService;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var products = (await cartService.GetDbCartProducts()).Data;
            decimal totalPrice = 0;
            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            var orderItems = new List<OrderItem>();
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice =  product.Price * product.Quantity
            }));

            var order = new Order
            {
                UserId = authService.GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            context.Orders.Add(order);
            context.CartItems.RemoveRange(context.CartItems.Where(ci => ci.UserId == authService.GetUserId()));
            await context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
