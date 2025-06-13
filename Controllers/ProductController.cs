using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Services;
using RigidboysAPI.Models;
using RigidboysAPI.Dtos;

namespace RigidboysAPI.Controllers
{
    [ApiController]
    [Route("api/Product")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            try
            {
                await _service.AddProductAsync(dto);
                return Ok(new { message = "제품이 등록되었습니다." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });  // HTTP 409 중복 에러
            }
        }
        [HttpGet("Product_Names")]
        public async Task<ActionResult<List<string>>> GetProductNames()
        {
            var names = await _service.GetProductNamesAsync();
            return Ok(names);
        }


    }
}
