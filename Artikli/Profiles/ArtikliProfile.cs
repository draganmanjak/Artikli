using Artikli.ViewModels;
using AutoMapper;
using DataAccess.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artikli.Profiles
{
    public class ArtikliProfile: Profile
    {
        public ArtikliProfile()
        {

            CreateMap<DataAccess.Infrastructure.Models.ArtikliModel, ArtikliViewModel>().ReverseMap();

            CreateMap<DataAccess.Infrastructure.Models.AtributiArtikla, AtributiArtiklaViewModel>().ReverseMap();
        }
    }
}
