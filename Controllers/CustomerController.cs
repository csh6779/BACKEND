using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Services;
using RigidboysAPI.Models;
using RigidboysAPI.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using RigidboysAPI.Errors;
using System.Linq.Expressions; // ✅ 이거 추가해야 Swagger 주석 작동

namespace RigidboysAPI.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _service;

        public CustomerController(CustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "고객 목록 조회",
            Tags = new[] { "고객 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "조회 성공", typeof(List<Customer>))]
        public async Task<ActionResult<List<Customer>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "고객을 등록합니다.",
            Tags = new[] { "고객 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "등록 성공")]
        [SwaggerResponse(400, "입력값 오류")]
        [SwaggerResponse(409, "중복된 고객")]
        [SwaggerResponse(500, "서버 오류")]
        public async Task<IActionResult> Create([FromBody] CustomerDto dto)
        {
            if (!ModelState.IsValid)
                return ErrorResponseHelper.HandleBadRequest(ModelState);
            try
            {
                await _service.AddCustomerAsync(dto);
                return Ok(new { message = "고객이 등록되었습니다." });
            }
            catch (ArgumentException) //400
            {
                return ErrorResponseHelper.HandleBadRequest(
                    ErrorCodes.INVALID_INPUT,
                    ErrorCodes.INVALID_INPUT_MESSAGE
                );
            }
            catch (InvalidOperationException) //409
            {
                return ErrorResponseHelper.HandleConflict(
                    ErrorCodes.DUPLICATE_CUSTOMER,
                    ErrorCodes.DUPLICATE_CUSTOMER_MESSAGE
                );
            }
            catch (Exception ex) //500
            {
                return ErrorResponseHelper.HandleServerError(
                    ErrorCodes.SERVER_ERROR,
                    ex.Message
                );
            }
        }
        


        [HttpGet("Office_Name")]
        [SwaggerOperation(
            Summary = "고객사 이름만 조회합니다.",
            Tags = new[] { "고객 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "고객사의 이름 목록 조회 성공", typeof(List<string>))]
        public async Task<ActionResult<List<string>>> GetCustomerNames()
        {
            var names = await _service.GetCustomerNamesAsync();
            return Ok(names);
        }
    }
}
