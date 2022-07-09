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

        public int CurrentPage { get; set; } = 1;

        public int PageCount { get; set; } = 0;

        public  string LastSearchText { get; set; } = string.Empty;
        public List<Product> AdminProducts { get; set; }

        public event Action ProductsChanged;

        public async Task<Product> CreateProduct(Product product)
        {
            var result = await httpClient.PostAsJsonAsync("api/product", product);
            var newProduct = (await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
            return newProduct;
        }

        public async Task DeleteProduct(Product product)
        {
            var result = await httpClient.DeleteAsync($"api/product/{product.Id}");
        }

        public async Task GetAdminProducts()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/admin");
            AdminProducts = result.Data;
            CurrentPage = 1;
            PageCount = 0;
            if (AdminProducts.Count == 0)
                Message = "No Products Found.";
        }

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

            CurrentPage = 1;
            PageCount = 0;

            if (Products.Count == 0)
                Message = "No Products found";

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductsSearchSuggestions(string searchText)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task SearchProducts(string searchText, int page)
        {
            LastSearchText = searchText;   
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<ProductSearchResult>>($"api/product/search/{searchText}/{page}");
            if (result != null && result.Data != null)
            {
                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }
            if (Products.Count == 0) Message = "No Products Found.";
            ProductsChanged?.Invoke();

        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var result = await httpClient.PutAsJsonAsync($"api/product", product);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
        }
    }
}
