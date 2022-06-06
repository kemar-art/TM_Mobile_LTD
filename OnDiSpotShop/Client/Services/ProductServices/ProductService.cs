namespace OnDiSpotShop.Client.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public List<Product> Products { get; set; } = new List<Product>();
        public string Message { get; set; } = "Loading Products...";

        public event Action ProductsChanged;

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return result;
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = categoryUrl == null ?
                await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured") :
                await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
            if (result != null && result.Data != null)
            Products = result.Data;

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductsSearchSuggestions(string searchText)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task SearchProducts(string searchText)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{searchText}");
            if (result != null && result.Data != null)
                Products = result.Data;
            if (Products.Count == 0) Message = "No Products Found.";
            ProductsChanged?.Invoke();

        }
    }
}
