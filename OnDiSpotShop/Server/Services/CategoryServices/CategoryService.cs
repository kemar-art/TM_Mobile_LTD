namespace OnDiSpotShop.Server.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext context;

        public CategoryService(DataContext context)
        {
            this.context = context;
        }

        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            var Categories = await context.Categories.ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = Categories
            };
        }
    }
}
