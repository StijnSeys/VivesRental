using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VivesRental.Model;
using VivesRental.Services.Contracts;

namespace RentalFrontend.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult CreateProduct()
        {
            return View();

        }
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _productService.Create(product);

            return RedirectToAction("CreateProduct");

        }

        
    }
}