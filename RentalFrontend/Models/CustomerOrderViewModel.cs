using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VivesRental.Model;
using VivesRental.Repository.Results;

namespace RentalFrontend.Models
{
    public class CustomerOrderViewModel
    {

        public bool extraInfo { get; set; }

        public int Error { get; set; }
        public string Message { get; set; }
        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }

        public Order Order { get; set; }

        public Article Article { get; set; }

        public Product Product { get; set; }
        public IList<OrderResult> OrderResults { get; set; }

        public IList<ProductResult> ProductResults { get; set; }

        public IList<Article> AvailableArticles { get; set; }

        public IList<OrderLine> OrderLines { get; set; }

        public IList<Customer> AllCustomers { get; set; }

        public IList<Article> OrderArticles { get; set; }
    }
}