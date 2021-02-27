using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Infrastructure.Models
{
    public class AtributiArtikla
    {
        [Key]
        public long PkFkArtikalId { get; set; }
        [ForeignKey("PkFkArtikalId")]
        public ArtikliModel Artikal { get; set; }

        [Key]
        public long PkFkAtributId { get; set; }
        [ForeignKey("PkFkAtributId")]

        public Atributi Atribut { get; set; }
        public string Value { get; set; }


    }
}
