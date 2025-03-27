using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Restorant.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        [ValidateNever]
        public Category? Category {get; set;}
        public int CategoryId {get; set;}
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string ImageUrl { get; set; }
        [ValidateNever]
        public List<OrderItem> OrderItems { get; set; }
        [ValidateNever]
        public List<ProductIngredient> ProductIngredients { get; set; }
    }
}