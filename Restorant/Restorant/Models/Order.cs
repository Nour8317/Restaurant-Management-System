using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Restorant.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
