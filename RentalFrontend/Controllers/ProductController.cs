using System;
using Microsoft.AspNetCore.Mvc;
using RentalFrontend.Models;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Services.Contracts;

namespace RentalFrontend.Controllers
{
    public class ProductController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IProductService _productService;

        public ProductController(IProductService productService, IArticleService articleService)
        {
            _productService = productService;
            _articleService = articleService;
        }

        public IActionResult Index()
        {
            var model = new ArticleViewModel();

            var includes = new ProductIncludes
            {
                Articles = true
            };

            var products = _productService.AllResult(includes);

            model.Products = products;
            return View(model);
        }

        [HttpGet]
        public IActionResult ProductDetails(ArticleViewModel model)
        {
            var product = new Product();
            var includes = new ProductIncludes
            {
                Articles = true
            };
            if (model.ProductId == Guid.Empty)
                product = _productService.Get(model.Product.Id, includes);
            else
                product = _productService.Get(model.ProductId, includes);

            model.Product = product;

            return View(model);
        }


        public IActionResult CreateProduct()
        {
            var dummyProduct = new Product();
            return View(dummyProduct);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (!ModelState.IsValid) return View(product);

            _productService.Create(product);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var product = _productService.Get(id);

            if (product == null) return RedirectToAction("Index");

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid) return View(product);

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