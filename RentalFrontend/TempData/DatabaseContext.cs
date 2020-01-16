using System.Collections.Generic;
using VivesRental.Model;

namespace RentalFrontend.TempData
{
    public class DatabaseContext : IDatabaseContext
    {
        public IList<Article> Articles { get; set; }
    }
}