using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Infrastructure.Models
{
    public class ArtikliModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PkArtikliId { get; set; }
        [Required]
        public string Naziv { get; set; }
        [Required]
        public string Sifra { get; set; }
        [Required]
        public long FkJedinicaMjereId { get; set; }
        [ForeignKey("FkJedinicaMjereId")]
        public JediniceMjere JedinicaMjere { get; set; }

        public ICollection<AtributiArtikla> AtributiArtikla { get; set; }
    }
}
