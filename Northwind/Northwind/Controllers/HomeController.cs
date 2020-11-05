using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Filters;
using Northwind.Data.Domain.Model;
using Northwind.DataAccess.Interfaces;
using Northwind.Models;
using System.Diagnostics;

namespace Northwind.Controllers
{
    [ServiceFilter(typeof(LoggingFilter))]
    public class HomeController : Controller
    {
        private readonly IRepository<Employee> _employeeRepository;

        public HomeController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

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
