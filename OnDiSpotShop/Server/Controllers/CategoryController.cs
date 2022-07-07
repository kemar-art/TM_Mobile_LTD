using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnDiSpotShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategories()
        {
            var result = await categoryService.GetCategories();
            return Ok(result);
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetAdminCategories()
        {
            var result = await categoryService.GetAdminCategories();
            return Ok(result);
        }

        [HttpDelete("admin/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> DeleteCategory(int id)
        {
            var result = await categoryService.DeleteCategory(id);
            return Ok(result);
        }

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> AddCategory(Category category)
        {
            var result = await categoryService.AddCategory(category);
            return Ok(result);
        }

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> UpdateCategory(Category category)
        {
            var result = await categoryService.UpdateCategory(category);
            return Ok(result);
        }
    }
}
