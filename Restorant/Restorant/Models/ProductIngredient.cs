using System.ComponentModel.DataAnnotations;

namespace Restorant.Models
{
    public class ProductIngredient
    {
        [Key]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

    } 
}