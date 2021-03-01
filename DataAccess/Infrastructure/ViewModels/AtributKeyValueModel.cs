using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Infrastructure.ViewModels
{
    public class AtributKeyValueModel
    {
        public long PkFkArtikalId { get; set; }
        public long PkFkAtributId { get; set; }
        public string Value { get; set; }


    }
}
