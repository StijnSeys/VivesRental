using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VivesRental.Model;
using VivesRental.Services.Contracts;

namespace RentalFrontend.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {

            _customerService = customerService;
        }


        public IActionResult CustomerList()
        {

            var customers = _customerService.All();

            return View(customers);
        }

        [HttpGet]
        public IActionResult CustomerDetails(Guid id)
        {
            var customer = _customerService.Get(id);

            return View(customer);

        }


        public IActionResult Create()
        {
            var dummyCustomer = new Customer();
            return View(dummyCustomer);
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            _customerService.Create(customer);

            return RedirectToAction("CustomerList");

        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var product = _customerService.Get(id);

            if (product == null)
            {
                return RedirectToAction("CustomerList");
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            _customerService.Edit(customer);

            return RedirectToAction("CustomerList");
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {

            _customerService.Remove(id);

            return RedirectToAction("CustomerList");
        }
    }
}