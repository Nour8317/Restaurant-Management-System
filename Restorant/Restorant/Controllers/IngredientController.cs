using Microsoft.AspNetCore.Mvc;
using Restorant.Data;
using Restorant.Models;

namespace Restorant.Controllers
{
    public class IngredientController : Controller
    {
        private Repository<Ingredient> ingredients;
        public IngredientController(ApplicationDbContext context)
        {
            this.ingredients = new Repository < Ingredient>(context);
        }
        public async Task<IActionResult> Index()
        {
            return View(await ingredients.GetAllAsync());
        }
    }
}
