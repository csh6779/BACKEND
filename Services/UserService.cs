using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegisterAsync(UserDto dto)
    {
        // 중복 아이디 검사
        bool exists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
        if (exists)
            throw new InvalidOperationException("이미 존재하는 사용자입니다.");

        var user = new User
        {
            Name = dto.Name,
            UserId = dto.UserId,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
    public async Task<string> LoginAsync(LoginDto dto, JwtService jwtService)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.UserId);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("아이디 또는 비밀번호가 올바르지 않습니다.");

        return jwtService.GenerateToken(user.Id, user.Role);
    }
    public async Task<List<User>> GetAllAsync()
    {
    return await _context.Users.ToListAsync();
    }
    public async Task<User> FindByUserIdAsync(string userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId)
           ?? throw new InvalidOperationException("사용자를 찾을 수 없습니다.");
    }
}
