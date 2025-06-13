#nullable enable
namespace RigidboysAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Office_Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Master_Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Description{ get; set; }
    }
}
