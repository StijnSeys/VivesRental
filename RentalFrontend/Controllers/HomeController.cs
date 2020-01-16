using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RentalFrontend.Models;

namespace RentalFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int customerFound)
        {
            if (customerFound == 1)
            {
                CustomerOrderViewModel vm = new CustomerOrderViewModel()
                {
                    Message = "Customer not found",
                    Error = customerFound
                };

                return View(vm);
            }

            else
            {
                CustomerOrderViewModel vm = new CustomerOrderViewModel()
                {
                    Message = "",
                    Error = 2
                };

                return View(vm);
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}