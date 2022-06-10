namespace OnDiSpotShop.Server.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly DataContext context;

        public CartService(DataContext context)
        {
            this.context = context;
        }
        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponse>>
            {
                Data = new List<CartProductResponse>()
            };

            foreach (var item in cartItems)
            {
                var product = await context.Products
                    .Where(p => p.Id == item.ProductId)
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    continue;
                }

                var productVarint = await context.ProductVariants
                    .Where(v => v.ProductId == item.ProductId
                          && v.ProductTypeId == item.ProductTypeId)
                    .Include(v => v.ProductType)
                    .FirstOrDefaultAsync();

                if (productVarint == null)
                {
                    continue;
                }

                var cartProduct = new CartProductResponse
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = productVarint.Price,
                    ProductType = productVarint.ProductType.Name,
                    ProductTypeId = productVarint.ProductTypeId,
                    Quantity = item.Quantity
                };

                result.Data.Add(cartProduct);
            }
            
            return result;
        }
    }
}
