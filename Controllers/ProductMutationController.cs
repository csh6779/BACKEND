using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Dtos;
using RigidboysAPI.Services;

namespace RigidboysAPI.Controllers
{
    [ApiController]
    [Route("api/products/mutation")]
    public class ProductMutationController : ControllerBase
    {
        private readonly ProductMutationService _mutationService;

        public ProductMutationController(ProductMutationService mutationService)
        {
            _mutationService = mutationService;
        }

        // ✅ 고객 정보 수정
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
        {
           try
            {
                await _mutationService.UpdateAsync(id, dto);
                return Ok("제품 정보 수정 완료");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ 고객 정보 삭제
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mutationService.DeleteAsync(id);
            return Ok("제품 삭제 완료");
        }
    }
}
