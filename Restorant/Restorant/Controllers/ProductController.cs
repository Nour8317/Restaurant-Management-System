using Microsoft.AspNetCore.Mvc;
using Restorant.Data;
using Restorant.Models;

namespace Restorant.Controllers
{
    public class ProductController : Controller
    {
        Repository<Product> products;
        public ProductController(ApplicationDbContext context) {
            this.products = new Repository<Product>(context);        
        }
        public async Task<IActionResult> Index()
        { 
            return View(await products.GetAllAsync());
        }
        

    }
}
