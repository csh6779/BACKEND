#nullable enable
namespace RigidboysAPI.Dtos
{
    public class ProductDto
    {
        public string Product_name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string License { get; set; } = string.Empty;
        public string Product_price { get; set; } = string.Empty;
        public string Production_price { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}