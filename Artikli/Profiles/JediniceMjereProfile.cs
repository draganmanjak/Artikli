using Artikli.ViewModels.Models;
using Artikli.ViewModels;
using AutoMapper;
using DataAccess.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artikli.Profiles
{
    public class JediniceMjereProfile : Profile
    {
        public JediniceMjereProfile()
        {
            CreateMap<JediniceMjere, JediniceMjereViewModel>().ReverseMap();



        }
    }
}
