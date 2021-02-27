using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Infrastructure.Models
{
    public class Atributi
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PkAtributId { get; set; }
        [Required]
        public string Naziv { get; set; }

        public long? FkJedinicaMjereId { get; set; }

        public string JedinicaMjere { get; set; }

        public ICollection<AtributiArtikla> ArtikliAtributa { get; set; }

    }
}
