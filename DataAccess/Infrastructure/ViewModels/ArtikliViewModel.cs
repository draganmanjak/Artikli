
using DataAccess.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Infrastructure.ViewModels
{
    public class ArtikliViewModel
    {
      
        public long PkArtikliId { get; set; }
        [Required]
        public string Naziv { get; set; }
        [Required]
        public string Sifra { get; set; }
        [Required]
        public double Cijena { get; set; }

        public long? FkJedinicaMjereId { get; set; }
        public string JedinicaMjere { get; set; }

        public ICollection<AtributiViewModel> AtributiArtikla { get; set; }
        public ICollection<AtributiArtiklaViewModel> AtributiArtiklaViewModelList { get; set; }
    }
}
