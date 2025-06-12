using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RigidboysAPI.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddProductAsync(ProductDto dto)
        {
            bool exists = await _context.Products.AnyAsync(p => p.Product_name == dto.Product_name);
            if (exists)
            {
                 throw new InvalidOperationException("이미 등록된 제품명입니다.");
            }
            var entity = new Product
            {
                Product_name = dto.Product_name,
                Category = dto.Category,
                License = dto.License,
                Product_price = dto.Product_price,
                Description = dto.Description
            };

            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}
