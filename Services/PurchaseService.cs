using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RigidboysAPI.Services
{
    public class PurchaseService
    {
        private readonly AppDbContext _context;

        public PurchaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> GetAllAsync()
        {
            return await _context.Purchase.ToListAsync();
        }

        public async Task AddProductAsync(PurchaseDto dto)
        {
            bool exists = await _context.Purchase.AnyAsync(p => p.OfficeName == dto.OfficeName);
            if (exists)
            {
                 throw new InvalidOperationException("이미 등록된 제품명입니다.");
            }
            var entity = new Purchase
            {
                Purchase_or_Sale= dto.Purchase_or_Sale,
                OfficeName = dto.OfficeName,
                Date = dto.Date,
                Product_Name = dto.Product_Name,
                Amount = dto.Amount,
                Price = dto.Price,
                DeadLine = dto.DeadLine,
                PayDone = dto.PayDone,
                Is_Payment = dto.Is_Payment,
                Description = dto.Description,
                Paid_Payment = dto.Paid_Payment
            };

            _context.Purchase.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}
