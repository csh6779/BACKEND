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

        public async Task AddOrUpdatePurchaseAsync(PurchaseDto dto)
{
    // 중복 조건: 거래방식 + 고객사명 + 거래일 + 제품명 + 납기일
    var existing = await _context.Purchase.FirstOrDefaultAsync(p =>
        p.Purchase_or_Sale == dto.Purchase_or_Sale &&
        p.OfficeName == dto.OfficeName &&
        p.Date == dto.Date &&
        p.Product_Name == dto.Product_Name &&
        p.DeadLine == dto.DeadLine
    );

    if (existing != null)
    {
        // 기존 데이터 존재 → 누적
        existing.Amount += dto.Amount ?? 0;
        existing.Price += dto.Price ?? 0;
        existing.Paid_Payment += dto.Paid_Payment ?? 0;

        existing.Is_Payment = dto.Is_Payment;
        existing.PayDone = dto.PayDone;
        existing.Description = dto.Description;

        _context.Purchase.Update(existing);
    }
    else
    {
        // 새 데이터 삽입
        var entity = new Purchase
        {
            Purchase_or_Sale = dto.Purchase_or_Sale,
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

        await _context.Purchase.AddAsync(entity);
    }

    await _context.SaveChangesAsync();
}

    }
}
