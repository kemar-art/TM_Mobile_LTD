using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace OnDiSpotShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext context;

        public ProductController(DataContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            var products = await context.Products.ToListAsync();
            return Ok(products);
        }
    }
}
