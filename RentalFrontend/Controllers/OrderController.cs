using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RentalFrontend.Models;
using RentalFrontend.TempData;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Services.Contracts;

namespace RentalFrontend.Controllers
{
    public class OrderController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICustomerService _customerService;
        private readonly IDatabaseContext _databaseContext;
        private readonly IOrderLineService _orderLineService;

        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrderController(IOrderService orderService,
            IOrderLineService orderLineService,
            IProductService productService,
            IArticleService articleService,
            ICustomerService customerService,
            IDatabaseContext databaseContext
        )
        {
            _orderLineService = orderLineService;
            _orderService = orderService;
            _productService = productService;
            _articleService = articleService;
            _customerService = customerService;
            _databaseContext = databaseContext;
        }


        public IActionResult Index(CustomerOrderViewModel model)
        {
            var customer = _customerService.Get(model.Customer.Id);

            if (customer == null)
            {
                ModelState.AddModelError("Customer", "Customer not found");
                return RedirectToAction("Index");
            }

            model.Customer = customer;

            var productResults = _productService.GetAvailableProductResults();

            if (productResults == null) return View(model);

            model.ProductResults = productResults;

            var availableArticles = _articleService.GetAvailableArticles();

            model.AvailableArticles = availableArticles;
            return View("index", model);
        }

        public IActionResult AddOrderLine(CustomerOrderViewModel model)
        {
            var articleList = _databaseContext.Articles ?? new List<Article>();

            var includes = new ArticleIncludes
            {
                Product = true
            };

            //for scanned articleId
            if (model.Article!= null)
            {
                var article = _articleService.Get(model.Article.Id, includes);
                articleList.Add(article);
                model.Article = article;
            }
            // when clicked on order article button
            else
            {

                var availableArticle = _articleService.GetAvailableArticles(includes);
                IList<Article> list = availableArticle.Where(article => article.ProductId == model.Product.Id).ToList();

                var articlesAvailable = list.Except(articleList).ToList();

                if (articlesAvailable.Count == 0)
                {
                    var message = "This product is out of stock";
                    TempData["FeedbackMessage"] = message;

                }
                else
                {
                    var orderArticle = articlesAvailable.FirstOrDefault();

                    articleList.Add(orderArticle);
                }

            }

            model.OrderArticles = articleList;

            _databaseContext.Articles = articleList;

            return Index(model);
        }

        public IActionResult DeleteOrderLine(CustomerOrderViewModel model)
        {
            var orderArticles = _databaseContext.Articles;
            foreach (var article in orderArticles.ToList().Where(article => article.Id == model.Article.Id))
                orderArticles.Remove(article);
            model.OrderArticles = orderArticles;

            return Index(model);
        }

        [HttpPost]
        public IActionResult CreateOrder(CustomerOrderViewModel model)
        {
            model.Order = _orderService.Create(model.Customer.Id);

            var articles = _databaseContext.Articles;
            IList<Guid> articleIdS = articles.Select(article => article.Id).ToList();
            _orderLineService.Rent(model.Order.Id, articleIdS);

            _databaseContext.Articles = new List<Article>();

            var message = "Order created";
            TempData["FeedbackMessage"] = message;

            return RedirectToAction("CustomerDetails", "Customer", new {CustomerID = model.Customer.Id});

        }


        [HttpGet]
        public IActionResult GetOrder(CustomerOrderViewModel model)
        {
            var order = _orderService.Get(model.Order.Id);
            var orderLines = _orderLineService.FindByOrderId(model.Order.Id);

            model.Order = order;
            model.OrderLines = orderLines;

            return View(model);
        }

        [HttpGet]

        public IActionResult OrderList()
        {

            var model = new CustomerOrderViewModel();

           var orderResults =  _orderService.AllResult();

           model.OrderResults = orderResults;

            return View(model);
        }

        [HttpPost]
        public IActionResult ReturnArticle(CustomerOrderViewModel model)
        {
            var dateTime = DateTime.Now;


            //No easy way  13/01??
            var includes = new ArticleIncludes
            {
                OrderLines = true
            };
            var rentArticles = _articleService.GetRentedArticles(model.Customer.Id, includes);
            var orderLineId = new Guid();
            foreach (var article in rentArticles)
            {
                var orderLines = article.OrderLines;
                foreach (var orderLine in orderLines)
                    if (orderLine.ArticleId == model.Article.Id)
                        orderLineId = orderLine.Id;
            }

            _orderLineService.Return(orderLineId, dateTime);

            var message = "Article returned";
            TempData["FeedbackMessage"] = message;

            return RedirectToAction("CustomerDetails", "Customer", new {CustomerID = model.Customer.Id});
        }

        [HttpPost]
        public IActionResult ReturnFullOrder(CustomerOrderViewModel model)
        {
            var dateTime = DateTime.Now;

            _orderService.Return(model.Order.Id, dateTime);

            var message = "Order returned";
            TempData["FeedbackMessage"] = message;
            if (model.ExtraInfo)
            {
              return  RedirectToAction("OrderList");
            }

            return RedirectToAction("CustomerDetails", "Customer", new { CustomerID = model.Customer.Id });

        }
    }
}