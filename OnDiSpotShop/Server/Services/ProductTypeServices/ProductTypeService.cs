namespace OnDiSpotShop.Server.Services.ProductTypeServices
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext context;

        public ProductTypeService(DataContext context)
        {
            this.context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
        {
            productType.Editing = productType.IsNew = false;
            context.ProductTypes.Add(productType);
            await context.SaveChangesAsync();

            return await GetProductTypes();
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await context.ProductTypes.ToListAsync();
            return new ServiceResponse<List<ProductType>> { Data = productTypes };
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            var dbProductType = await context.ProductTypes.FindAsync(productType.Id);
            if (dbProductType == null)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Product Type not found."
                };
            }

            dbProductType.Name = productType.Name;
            await context.SaveChangesAsync();

            return await GetProductTypes();
        }
    }
}
