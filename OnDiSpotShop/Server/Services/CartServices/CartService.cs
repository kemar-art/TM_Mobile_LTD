using System.Security.Claims;

namespace OnDiSpotShop.Server.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly DataContext context;
        private readonly IAuthService authService;

        public CartService(DataContext context, IAuthService authService)
        {
            this.context = context;
            this.authService = authService;
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

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.UserId = authService.GetUserId());
            context.CartItems.AddRange(cartItems);
            await context.SaveChangesAsync();

            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var count = (await context.CartItems.Where(ci => ci.UserId == authService.GetUserId()).ToListAsync()).Count;
            return new ServiceResponse<int> { Data = count };
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts()
        {
            return await GetCartProducts(await context.CartItems.Where(ci => ci.UserId == authService.GetUserId()).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = authService.GetUserId();

            var sameItem = await context.CartItems.FirstOrDefaultAsync
                (ci => ci.ProductId == cartItem.ProductId && 
                 ci.ProductTypeId == cartItem.ProductTypeId &&
                 ci.UserId == cartItem.UserId
                );
            if (sameItem == null)
            {
                context.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var dbCartItem = await context.CartItems.FirstOrDefaultAsync
                (ci => ci.ProductId == cartItem.ProductId &&
                 ci.ProductTypeId == cartItem.ProductTypeId &&
                 ci.UserId == authService.GetUserId()
                );
            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist"
                };
            }

            dbCartItem.Quantity = cartItem.Quantity;
            await context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var dbCartItem = await context.CartItems.FirstOrDefaultAsync
                (ci => ci.ProductId == productId &&
                 ci.ProductTypeId == productTypeId &&
                 ci.UserId == authService.GetUserId()
                );
            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist"
                };
            }

            context.CartItems.Remove(dbCartItem);
            await context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data= true };
        }
    }
}
