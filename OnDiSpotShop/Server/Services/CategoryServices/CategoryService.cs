﻿namespace OnDiSpotShop.Server.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext context;

        public CategoryService(DataContext context)
        {
            this.context = context;
        }

        public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
        {
            category.Editing = category.IsNew = false;
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategory(int id)
        {
            Category category = await GetCategoryById(id);
            if (category == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            category.Deleted = true;
            await context.SaveChangesAsync();

            return await GetAdminCategories();
        }

        private async Task<Category> GetCategoryById(int id)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
        {
            var Categories = await context.Categories
                 .Where(c => !c.Deleted)
                 .ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = Categories
            };
        }

        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            var Categories = await context.Categories
                 .Where(c => !c.Deleted && c.Visible)
                 .ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = Categories
            };
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
        {
            var dbCategory = await GetCategoryById(category.Id);
            if (dbCategory == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;

            await context.SaveChangesAsync();

            return await GetAdminCategories();

        }
    }
}
