using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RentalFrontend.Models;
using VivesRental.Model;
using VivesRental.Services.Contracts;

namespace RentalFrontend.Controllers
{
    public class ArticleController : Controller
    {

        private readonly IArticleService _articleService;
        private readonly IProductService _productService;
        public ArticleController(IArticleService articleService, IProductService productService)
        {

            _articleService = articleService;
            _productService = productService;
        }

    

        public IActionResult ArticleList(Guid id)
        {

            var product = _productService.Get(id);

            var articles = product.Articles;

            ArticleViewModel model = new ArticleViewModel();
            model.Articles = articles;
            model.ProductId = id;
            

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateArticle(ArticleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                RedirectToPage("ArticleList");
            }

            Article article = new Article
            {
                ProductId = model.ProductId,
                Status = 0
            };

            for (int i = 0; i < model.Count; i++)
            {
                _articleService.Create(article);
            }
            

            return RedirectToAction("ArticleList", new{ id= model.ProductId});

        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {

            _productService.Remove(id);

            return RedirectToAction("ArticleList");
        }
    }
}