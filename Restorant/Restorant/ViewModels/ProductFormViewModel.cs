using Microsoft.AspNetCore.Mvc.Rendering;
using Restorant.Models;

namespace Restorant.ViewModels
{
    public class ProductFormViewModel
    {
        public Product Product {  get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<IngredientCheckbox> Ingredients { get; set; }
    }
    public class IngredientCheckbox
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
