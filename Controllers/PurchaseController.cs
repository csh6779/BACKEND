using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Services;
using RigidboysAPI.Models;
using RigidboysAPI.Dtos;

namespace RigidboysAPI.Controllers
{
    [ApiController]
    [Route("api/Purchase")]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _service;

        public PurchaseController(PurchaseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Purchase>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseDto dto)
        {
            try
            {
                await _service.AddOrUpdatePurchaseAsync(dto);  // 이전에 만든 병합 로직
                return Ok(new { message = "매입/매출 정보가 저장되었습니다." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }
    }
}
