using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Dtos;
using RigidboysAPI.Services;

namespace RigidboysAPI.Controllers
{
    [ApiController]
    [Route("api/customers/mutation")]
    public class CustomerMutationController : ControllerBase
    {
        private readonly CustomerMutationService _mutationService;

        public CustomerMutationController(CustomerMutationService mutationService)
        {
            _mutationService = mutationService;
        }

        // ✅ 고객 정보 수정
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDto dto)
        {
            await _mutationService.UpdateAsync(id, dto);
            return Ok("고객 정보 수정 완료");
        }

        // ✅ 고객 정보 삭제
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mutationService.DeleteAsync(id);
            return Ok("고객 삭제 완료");
        }
    }
}
