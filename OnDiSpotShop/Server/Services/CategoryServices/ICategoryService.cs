namespace OnDiSpotShop.Server.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategories();
    }
}
