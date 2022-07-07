namespace OnDiSpotShop.Client.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient httpClient;

        public CategoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Category> AdminCategories { get; set; } = new List<Category>();

        public event Action OnChange;

        public async Task AddCategory(Category category)
        {
            var response = await httpClient.PostAsJsonAsync("api/Category/admin", category);
            AdminCategories = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategories();
            OnChange.Invoke();
        }

        public Category CreateNewCategory()
        {
            var newCategory = new Category { IsNew = true, Editing = true };
            AdminCategories.Add(newCategory);
            OnChange.Invoke();
            return newCategory;
        }

        public async Task DeleteCategory(int categoryId)
        {
            var response = await httpClient.DeleteAsync($"api/Category/admin/{categoryId}");
            AdminCategories = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategories();
            OnChange.Invoke();
        }

        public async Task GetAdminCategories()
        {
            var response = await httpClient.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category/admin");
            if (response != null && response.Data != null)
                AdminCategories = response.Data;
        }

        public async Task GetCategories()
        {
            var response = await httpClient.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category");
            if (response != null && response.Data != null)
                Categories = response.Data;
        }

        public async Task UpdateCategory(Category category)
        {
            var response = await httpClient.PutAsJsonAsync("api/Category/admin", category);
            AdminCategories = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategories();
            OnChange.Invoke();
        }
    }
}

