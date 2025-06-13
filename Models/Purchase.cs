#nullable enable
using System.ComponentModel.DataAnnotations;

namespace RigidboysAPI.Models
{
    public class Purchase
    {
        public int id{ get; set; }
        public string Purchase_or_Sale { get; set; } = string.Empty;
        public string Office_Name { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public string Product_Name { get; set; } = string.Empty;
        public int? Amount { get; set; }
        public int? Price { get; set; }
        public DateTime? DeadLine { get; set; }
        public DateTime? PayDone { get; set; }
        public Boolean? Is_Payment { get; set; }
        public string? Description { get; set; }
        public int? Paid_Payment{ get; set; }
    }
}