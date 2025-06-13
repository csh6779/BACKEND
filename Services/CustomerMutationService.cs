using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RigidboysAPI.Services
{
    public class CustomerMutationService
    {
        private readonly AppDbContext _context;

        public CustomerMutationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);  // ✅ 복수형으로 수정
            if (customer == null)
                throw new InvalidOperationException("삭제할 고객사가 없습니다.");

            _context.Customers.Remove(customer);  // ✅ 변수 이름도 수정
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);  // ✅ 복수형으로 수정
            if (customer == null)
                throw new InvalidOperationException("수정할 고객사가 없습니다.");

            customer.Office_Name = dto.Office_Name;
            customer.Type = dto.Type;
            customer.Master_Name = dto.Master_Name;
            customer.Phone = dto.Phone;  // ✅ 실수 수정
            customer.Address = dto.Address;
            customer.Description = dto.Description;

            _context.Customers.Update(customer);  // ✅ 복수형으로 수정
            await _context.SaveChangesAsync();
        }
    }
}
