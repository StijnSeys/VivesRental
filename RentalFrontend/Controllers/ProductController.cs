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
        private readonly IArticleService _articleService;

        public ProductController(IProductService productService , IArticleService articleService)
        {
            _productService = productService;
            _articleService = articleService;
        }
        public IActionResult Index()
        {
            var products = _productService.All();
            return View(products);

        }

        [HttpGet]
        public IActionResult ProductDetails(Guid id)
        {
            var product = _productService.Get(id);

            return View(product);

        }


        public IActionResult CreateProduct()
        {
            var dummyProduct = new Product();
            return View(dummyProduct);
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

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var product = _productService.Get(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _productService.Edit(product);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {

            _productService.Remove(id);

            return RedirectToAction("Index");
        }
        
    }
}