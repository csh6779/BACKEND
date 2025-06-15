using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using RigidboysAPI.Models;
using RigidboysAPI.Dtos;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly JwtService _jwtService;

    public AuthController(UserService userService, JwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "로그인",
        Tags = new[] { "로그인 및 회원가입" }
    )]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var token = await _userService.LoginAsync(dto, _jwtService);
            var user = await _userService.FindByUserIdAsync(dto.UserId); // 사용자 정보 조회

            return Ok(new
            {
                token,
                userId = user.UserId,
                name = user.Name,
                role = user.Role
            });
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine(e.Message);
            return Unauthorized(new { message = "아이디 또는 비밀번호가 잘못되었습니다." });
        }
    }

    [Authorize(Roles = "Admin")] // ✅ 관리자만 접근 가능
    [HttpGet("all")]
    [SwaggerOperation(
        Summary = "회원가입된 사용자 목록 조회 (관리자 전용)",
        Tags = new[] { "로그인 및 회원가입" }
    )]
    [SwaggerResponse(200, "조회 성공", typeof(List<User>))]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }

    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "회원가입",
        Tags = new[] { "로그인 및 회원가입" }
    )]
    public async Task<IActionResult> Register([FromBody] UserDto dto)
    {
        try
        {
            await _userService.RegisterAsync(dto);
            return Ok(new { message = "회원가입 완료" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
