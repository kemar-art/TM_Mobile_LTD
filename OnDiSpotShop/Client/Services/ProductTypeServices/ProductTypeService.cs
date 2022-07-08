namespace OnDiSpotShop.Client.Services.ProductTypeServices
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient httpClient;

        public ProductTypeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();

        public event Action OnChange;

        public async Task AddProductType(ProductType productType)
        {
            var response = await httpClient.PostAsJsonAsync("api/producttype", productType);
            ProductTypes = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }

        public ProductType CreateNewProductType()
        {
            var newProductType = new ProductType { IsNew = true, Editing = true };

            ProductTypes.Add(newProductType);
            OnChange.Invoke();
            return newProductType;
        }

        public async Task GetProductTypes()
        {
            var result = await httpClient
                 .GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/producttype");
            ProductTypes = result.Data;
        }

        public async Task UpdateProductType(ProductType productType)
        {
            var response = await httpClient.PutAsJsonAsync("api/producttype", productType);
            ProductTypes = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }
    }
}
