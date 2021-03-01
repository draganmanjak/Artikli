using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Infrastructure.ViewModels
{
    public class AtributiViewModel
    {
        public long PkAtributId { get; set; }
        [Required]
        public string Naziv { get; set; }

        public long? FkJedinicaMjereId { get; set; }

        public string JedinicaMjere { get; set; }

        public string Value { get; set; }



    }
}
