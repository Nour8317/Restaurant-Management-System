using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Restorant.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }
        public string Name { get; set; }
        [ValidateNever]
        public List<ProductIngredient> ProductIngredients { get; set; }
    }
}