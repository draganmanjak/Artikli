using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Infrastructure.ViewModels
{
   public class PaginatedListViewModel
    {

        public List<ArtikliViewModel> Artikli { get; set; }
        public int PageNum { get;  set; }
        public int TotalPages { get;  set; }
    }

}
