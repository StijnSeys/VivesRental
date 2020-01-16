using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Results;

namespace RentalFrontend.Models
{
    public class ArticleViewModel
    {
        public bool FromGetArticle { get; set; }
        public Article Article { get; set; }

        public Product Product { get; set; }
        public int Count { get; set; }

        public Guid ProductId { get; set; }
        public Guid ArticleId { get; set; }

        public ArticleStatus status { get; set; }

        public IList<Article> Articles { get; set; }

        public IList<ProductResult> Products { get; set; }
    }
}