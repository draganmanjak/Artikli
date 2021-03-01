using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Infrastructure.ViewModels
{
    public class ArtikliSearchViewModel
    {
        public string Naziv { get; set; }
        public string Sifra { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }

        public ICollection<AtributiArtiklaViewModel> AtributiArtiklaViewModelList { get; set; }
    }
}
