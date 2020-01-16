using Microsoft.AspNetCore.Mvc;
using RentalFrontend.Models;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Services.Contracts;

namespace RentalFrontend.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public ArticleController(IArticleService articleService, IProductService productService,
            IOrderService orderService)
        {
            _articleService = articleService;
            _productService = productService;
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetArticle(ArticleViewModel model)
        {
            var includes = new ArticleIncludes
            {
                Product = true,
                OrderLines = true
            };

            var article = _articleService.Get(model.ArticleId, includes);

            foreach (var orderline in article.OrderLines)
            {
                var order = _orderService.Get(orderline.OrderId);
                orderline.Order = order;
            }

            model.Article = article;

            return View(model);
        }


        [HttpPost]
        public IActionResult CreateArticle(ArticleViewModel model)
        {
            if (!ModelState.IsValid) RedirectToPage("ArticleList");

            var article = new Article
            {
                ProductId = model.Product.Id,
                Status = 0
            };

            for (var i = 0; i < model.Count; i++) _articleService.Create(article);


            return RedirectToAction("ProductDetails", "Product", new {ProductId = model.Product.Id});
        }

        [HttpPost]
        public IActionResult ChangeStatus(ArticleViewModel model)
        {
            _articleService.UpdateStatus(model.ArticleId, model.status);

            if (model.FromGetArticle) return RedirectToAction("GetArticle", model);

            return RedirectToAction("ProductDetails", "Product", new {ProductId = model.Product.Id});
        }


        [HttpPost]
        public IActionResult Delete(ArticleViewModel model)
        {
            _articleService.Remove(model.ArticleId);

            return RedirectToAction("ProductDetails", "Product", new {ProductId = model.Product.Id});
        }
    }
}