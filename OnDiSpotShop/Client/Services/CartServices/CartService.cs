namespace OnDiSpotShop.Client.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService localStorage;
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authStateProvider;

        public CartService(ILocalStorageService localStorage, HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            this.localStorage = localStorage;
            this.httpClient = httpClient;
            this.authStateProvider = authStateProvider;
        }
        public event Action Onchange;

        public async Task AddToCart(CartItem cartItem)
        {
            if (await IsUserAuthenticated())
            {
                await httpClient.PostAsJsonAsync("api/cart/add", cartItem);
            }
            else
            {
                var cart = await localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    cart = new List<CartItem>();
                }
                var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId
                      && x.ProductTypeId == cartItem.ProductTypeId);
                if (sameItem == null)
                {
                    cart.Add(cartItem);
                }
                else
                {
                    sameItem.Quantity += cartItem.Quantity;
                }

                await localStorage.SetItemAsync("cart", cart);
            }
            
            await GetCartItemsCount();
        }

        public async Task GetCartItemsCount()
        {
            if (await IsUserAuthenticated())
            {
                var result = await httpClient.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result.Data;

                await localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await localStorage.GetItemAsync<List<CartItem>>("cart");
                await localStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
            }

            Onchange.Invoke();
        }

        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            if (await IsUserAuthenticated())
            {
                var response = await httpClient.GetFromJsonAsync<ServiceResponse<List<CartProductResponse>>>("api/cart");
                return response.Data;
            }
            else
            {
                var cartItems = await localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems == null)
                    return new List<CartProductResponse>();
                var response = await httpClient.PostAsJsonAsync("api/cart/products", cartItems);
                var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();
                return cartProducts.Data;
            }
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            if (await IsUserAuthenticated())
            {
                await httpClient.DeleteAsync($"api/cart/{productId}/{productTypeId}");
            }
            else
            {
                var cart = await localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.ProductId == productId
                     && x.ProductTypeId == productTypeId);
                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                    await localStorage.SetItemAsync("cart", cart); 
                }
            }
        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            var localCart = await localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart == null)
            {
                return;
            }

            await httpClient.PostAsJsonAsync("api/cart", localCart);

            if (emptyLocalCart)
            {
                await localStorage.RemoveItemAsync("cart");
            }
        }

        public async Task UpdateQuantity(CartProductResponse product)
        {
            if (await IsUserAuthenticated())
            {
                var request = new CartItem
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    ProductTypeId = product.ProductTypeId
                };

                await httpClient.PutAsJsonAsync("api/cart/update-quantity", request);
            }
            else
            {
                var cart = await localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.ProductId == product.ProductId
                     && x.ProductTypeId == product.ProductTypeId);
                if (cartItem != null)
                {
                    cartItem.Quantity = product.Quantity;
                    await localStorage.SetItemAsync("cart", cart);
                }
            }  
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
