using Artikli.ViewModels.Models;
using AutoMapper;
using DataAccess.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artikli.Profiles
{
    public class AtributiProfile: Profile
    {
        public AtributiProfile()
        {
       
            CreateMap<Atributi, AtributiViewModel>().ReverseMap();


        }
    }
}
