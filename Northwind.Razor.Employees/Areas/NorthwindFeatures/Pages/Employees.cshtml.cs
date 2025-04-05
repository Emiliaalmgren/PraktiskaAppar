using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.EntityModels;
using PraktiskaAppar;

namespace Northwind.Razor.Employees.MyFeature.Pages
{
    public class EmployeesModel : PageModel
    {
        private NorthwindDatabaseContext db;

        public EmployeesModel (NorthwindDatabaseContext injectedContext)
        {
            db = injectedContext;
        }

        public IEnumerable<Employee>? Employees {  get; set; }

        [BindProperty]
        public Employee? Employee { get; set; }
        public void OnGet()
        {
            ViewData["Title"] = "Northwind App - Employees";

            Employees = db.Employees.OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName);
        }
    }
}
