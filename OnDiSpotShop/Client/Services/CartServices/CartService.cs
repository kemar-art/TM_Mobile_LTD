﻿namespace OnDiSpotShop.Client.Services.CartServices
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
                Console.WriteLine("User is Authenticated");
            }
            else
            {
                Console.WriteLine("User is Not Authenticated");
            }

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
            await GetCartItemsCount();
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            await GetCartItemsCount();
            var cart = await localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            return cart;
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
            var cartItems = await localStorage.GetItemAsync<List<CartItem>>("cart");
            var response = await httpClient.PostAsJsonAsync("api/cart/products", cartItems);
            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();
            return cartProducts.Data;
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
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
                await GetCartItemsCount();
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

        private async Task<bool> IsUserAuthenticated()
        {
            return (await authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
