using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.EntityModels;
using Northwind.Mvc.Models;
using PraktiskaAppar;

namespace Northwind.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindDatabaseContext db;

        public HomeController(ILogger<HomeController> logger, 
            NorthwindDatabaseContext injectedContext)
        {
            _logger = logger;
            db = injectedContext;
        }

        public IActionResult Index()
        {
            HomeIndexViewModel model = new(

                VisitorCount: Random.Shared.Next(1, 1001),
                Categories: db.Categories.ToList(),
                Products: db.Products.ToList(),
                ProductCategory: db.Products.Include(p => p.Category).ToList()

            );

            return View(model);
        }

        public IActionResult CategoryPage(int? id)
        {
            var category = db.Categories.Include(c => c.Products)
                .FirstOrDefault(c => c.CategoryId == id);

            if (!id.HasValue)
            {
                return BadRequest("You have to select a category with value, it's empty here");
            }

            if (category is null)
            {
                return NotFound("There is no information in this category! Try another one.");
            }

            return View(category);
        }

        public IActionResult ProductDetail(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("You must pass a product ID in the route, " +
                    "for example, /Home/ProductDetail/21");
            }
            Product? model = db.Products.SingleOrDefault(p => p.ProductId == id);
            if(model is null)
            {
                return NotFound($"ProductId{id} not found");
            }
            return View(model);

        }

        public IActionResult ModelBindning()
        {
            return View(); //en sida med en formul�r
        }

        [HttpPost]
        public IActionResult ModelBindning(Thing thing)
        {
            HomeModelBindningViewModel model = new(
                thing,
                HasErrors: !ModelState.IsValid,
                ValidationErrors: ModelState.Values
                    .SelectMany(state => state.Errors)
                    .Select(error => error.ErrorMessage)
            );

            return View(thing); //en sida som visar det anv�ndaren skickade
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
