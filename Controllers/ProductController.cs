using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Services;
using RigidboysAPI.Models;
using RigidboysAPI.Dtos;
using Swashbuckle.AspNetCore.Annotations; // ✅ 이거 추가해야 Swagger 주석 작동
using RigidboysAPI.Errors;

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
        [SwaggerOperation(
            Summary = "제품 목록 조회",
            Tags = new[] { "제품 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "조회 성공", typeof(List<Product>))]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpPost]

        [SwaggerOperation(
            Summary = "제품을 추가합니다.",
            Tags = new[] { "제품 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "등록 성공")]
        [SwaggerResponse(400, "입력값 오류")]
        [SwaggerResponse(409, "중복된 제품")]
        [SwaggerResponse(500, "서버 오류")]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
             if (!ModelState.IsValid)
                return ErrorResponseHelper.HandleBadRequest(ModelState);
            try
            {
                await _service.AddProductAsync(dto);
                return Ok(new { message = "제품이 등록되었습니다." });
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
                    ErrorCodes.DUPLICATE_PRODUCT,
                    ErrorCodes.DUPLICATE_PRODUCT_MESSAGE
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
        [HttpGet("Product_Names")]
        [SwaggerOperation(
            Summary = "제품의 이름만 조회합니다.",
            Tags = new[] { "제품 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "제품의 이름 목록 조회 성공", typeof(List<string>))]
        public async Task<ActionResult<List<string>>> GetProductNames()
        {
            var names = await _service.GetProductNamesAsync();
            return Ok(names);
        }


    }
}
