using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Infrastructure.Models
{
    public class JediniceMjere
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PkJedinicaMjereId { get; set; }
        [Required]
        public string Naziv { get; set; }

    }
}
