using System.Collections.Generic;
using VivesRental.Model;

namespace RentalFrontend.TempData
{
    public interface IDatabaseContext
    {
        IList<Article> Articles { get; set; }
    }
}