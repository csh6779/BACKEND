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

        public async Task AddCustomerAsync(CustomerDto dto)
        {
            bool exists = await _context.Customers.AnyAsync(c => c.OfficeName == dto.OfficeName);
            if (exists)
            {
                throw new InvalidOperationException("이미 등록된 고객사입니다.");
            }
            var entity = new Customer
            {
                OfficeName = dto.OfficeName,
                Type = dto.Type,
                Master_name = dto.Master_name,
                Phone = dto.Phone,
                Address = dto.Address,
                Description = dto.Description
            };

            _context.Customers.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<string>> GetCustomerNamesAsync()
        {
            return await _context.Customers
                .Select(c => c.OfficeName)
                .Distinct()
                .ToListAsync();
        }
    }
}
