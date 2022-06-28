using System.Security.Claims;

namespace OnDiSpotShop.Server.Services.OrserServices
{
    public class OrderService : IOrderService
    {
        private readonly DataContext context;
        private readonly ICartService cartService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public OrderService(DataContext context, ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.cartService = cartService;
            this.httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

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
                UserId = GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            context.Orders.Add(order);
            context.CartItems.RemoveRange(context.CartItems.Where(ci => ci.UserId == GetUserId()));
            await context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
