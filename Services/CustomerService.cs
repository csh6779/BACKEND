using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RigidboysAPI.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task AddCustomerAsync(CustomerDto dto, int userId)
        {
            bool exists = await _context.Customers.AnyAsync(c => c.Office_Name == dto.Office_Name);
            if (exists)
            {
                throw new InvalidOperationException("이미 등록된 고객사입니다.");
            }
            var entity = new Customer
            {
                Office_Name = dto.Office_Name,
                Type = dto.Type,
                Master_Name = dto.Master_Name,
                Phone = dto.Phone,
                Address = dto.Address,
                Description = dto.Description,
                CreatedByUserId = userId
            };

            _context.Customers.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<string>> GetCustomerNamesAsync()
        {
            return await _context.Customers
                .Select(c => c.Office_Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Customer>> GetAllAsync(string role, int userId)
        {
        var query = _context.Customers.AsQueryable();
        if (role != "Admin")
            query = query.Where(c => c.CreatedByUserId == userId);

        return await query.ToListAsync();
        }
    }
}
