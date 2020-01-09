using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VivesRental.Model;

namespace RentalFrontend.Models
{
    public class ArticleViewModel
    {
      public int Count { get; set; }

      public Guid ProductId { get; set; }

      public Guid ArticleId { get; set; }

      public ArticleStatus status { get; set; }
      
      public IList<Article> Articles { get; set; }
    }
}
