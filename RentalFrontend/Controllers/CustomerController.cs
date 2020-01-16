using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RentalFrontend.Models;
using VivesRental.Model;
using VivesRental.Services.Contracts;

namespace RentalFrontend.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderLineService _orderLineService;
        private readonly IOrderService _orderService;


        public CustomerController(ICustomerService customerService, IOrderService orderService,
            IOrderLineService orderLineService)
        {
            _orderLineService = orderLineService;
            _customerService = customerService;
            _orderService = orderService;
        }


        public IActionResult CustomerList()
        {
            var customers = _customerService.All();
            var model = new CustomerOrderViewModel
            {
                AllCustomers = customers
            };
            return View(model);
        }


        public IActionResult CustomerDetails(CustomerOrderViewModel model)
        {
            var customer = _customerService.Get(model.CustomerId);

            if (customer == null)
            {
                return RedirectToAction("Index", "Home",new{customerfound = 1});
            }

            var orders = _orderService.All();

            IList<Order> customerOrders = new List<Order>();
            if (customer.Orders.Count > 0)
                foreach (var order in orders)
                {
                    var customerOrderLines = _orderLineService.FindByOrderId(order.Id);
                    order.OrderLines = customerOrderLines;

                    //no empty orders
                    if (order.CustomerId == model.CustomerId && order.OrderLines.Count > 0) customerOrders.Add(order);
                }

            model.Customer = customer;
            model.Customer.Orders = customerOrders;
            return View(model);
        }


        public IActionResult Create()
        {
            var dummyCustomer = new Customer();
            return View(dummyCustomer);
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);

            _customerService.Create(customer);

            return RedirectToAction("CustomerList");
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var product = _customerService.Get(id);

            if (product == null) return RedirectToAction("CustomerList");

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);

            _customerService.Edit(customer);

            return RedirectToAction("CustomerList");
        }

        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            _customerService.Remove(customer.Id);

            return RedirectToAction("CustomerList");
        }
    }
}