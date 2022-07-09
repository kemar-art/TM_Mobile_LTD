namespace OnDiSpotShop.Server.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly DataContext context;
        private readonly IHttpContextAccessor httpContext;

        public ProductService(DataContext context, IHttpContextAccessor httpContext)
        {
            this.context = context;
            this.httpContext = httpContext;
        }

        public async Task<ServiceResponse<Product>> CreateProduct(Product product)
        {
            foreach (var variant in product.Variants)
            {
                variant.ProductType = null;
            }

            context.Products.Add(product);
            await context.SaveChangesAsync();
            return new ServiceResponse<Product> { Data = product };
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
        {
            var dbProduct = await context.Products.FindAsync(productId);
            if (dbProduct == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "No Product Found."
                };
            }

            dbProduct.Deleted = true;
            await context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true};
        }

        public async Task<ServiceResponse<List<Product>>> GetAdminProducts()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await context.Products
                    .Where(p => !p.Deleted)
                    .Include(p => p.Variants.Where(v => !v.Deleted))
                    .ThenInclude(v => v.ProductType)
                    //.Include(p => p.)
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await context.Products
                       .Where(p => p.Featured && p.Visible && !p.Deleted)
                       .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                       .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            Product product = null;

            if (httpContext.HttpContext.User.IsInRole("Admin"))
            {
                product = await context.Products
                .Include(p => p.Variants.Where(v => !v.Deleted))
                .ThenInclude(v => v.ProductType)
                .FirstOrDefaultAsync(p => p.Id == productId && !p.Deleted);
            }
            else
            {
                product = await context.Products
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                .ThenInclude(v => v.ProductType)
                .FirstOrDefaultAsync(p => p.Id == productId && !p.Deleted && p.Visible);
            }

            if (product == null)
            {
                response.Success = false;
                response.Message = "This Item is no longer Available.";
            }
            else
            {
                response.Data = product;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await context.Products
                .Where(p => p.Visible && !p.Deleted)
                .Include(p => p.Variants).Where(v => v.Visible && !v.Deleted).ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await context.Products
                      .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower())
                           && p.Visible && !p.Deleted)
                      .Include(p => p.Variants)
                      .ToListAsync()
            };

            return response;
        }
        
        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText)
        {
            var products = await FindProductsBySearchText(searchText);

            List<string> result = new List<string>();

            foreach (var product in products)
            {
                if (product.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Name);
                }

                if (product.Description != null)
                {
                    var punctuation = product.Description.Where(char.IsPunctuation)
                        .Distinct().ToArray();
                    var words = product.Description.Split()
                         .Select(s => s.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if(word.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                            && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        // This is for the search bar.
        public async Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page)
        {
            var pageResults = 2f;
            var pageCount = Math.Ceiling((await FindProductsBySearchText(searchText)).Count / pageResults);
            var products = await context.Products
                            .Where(p => p.Name.ToLower().Contains(searchText.ToLower()) ||
                            p.Description.ToLower().Contains(searchText.ToLower())
                               && p.Visible && !p.Deleted)
                            .Include(p => p.Variants)
                            .Skip((page - 1) * (int)pageResults)
                            .Take((int)pageResults)
                            .ToListAsync();

            var response = new ServiceResponse<ProductSearchResult>
            {
                Data = new ProductSearchResult
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> UpdateProduct(Product product)
        {
            var dbProduct = await context.Products.FindAsync(product.Id);
            if (dbProduct == null)
            {
                return new ServiceResponse<Product>
                {
                    Success = false,
                    Message = "No Product Found."
                };
            }

            dbProduct.Name = product.Name;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;

            foreach (var varint in product.Variants)
            {
                var dbVarint = await context.ProductVariants.SingleOrDefaultAsync(v => v.ProductId == varint.ProductId
                    && v.ProductTypeId == varint.ProductTypeId);
                if (dbVarint == null)
                {
                    varint.ProductType = null;
                    context.ProductVariants.Add(varint);
                }
                else
                {
                    dbVarint.ProductTypeId = varint.ProductTypeId;
                    dbVarint.Price = varint.Price;
                    dbVarint.OriginalPrice = varint.OriginalPrice;
                    dbVarint.Visible = varint.Visible;
                    dbVarint.Deleted = varint.Deleted;
                }
            }

            await context.SaveChangesAsync();
            return new ServiceResponse<Product> { Data = product };
        }

        private async Task<List<Product>> FindProductsBySearchText(string searchText)
        {
            return await context.Products
                            .Where(p => p.Name.ToLower().Contains(searchText.ToLower()) ||
                            p.Description.ToLower().Contains(searchText.ToLower())
                                && p.Visible && !p.Deleted)
                            .Include(p => p.Variants)
                            .ToListAsync();
        }
    }
}
