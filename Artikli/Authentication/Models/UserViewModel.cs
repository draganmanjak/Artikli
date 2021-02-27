using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Artikli.Authentication.Models
{
    public class UserViewModel
    {
        public int PkUserId { get; set; }
        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string BrojTelefona { get; set; }
        [Required]
        public string Password { get; set; }
    
    }
}
