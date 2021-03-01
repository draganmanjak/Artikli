using AutoMapper;
using DataAccess.Infrastructure.Generic;
using DataAccess.Infrastructure.Interfaces;
using DataAccess.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Infrastructure.Services
{
    public class AtributiArtiklaRepository : GenericRepository<AtributiArtikla>, IAtributiArtiklaRepository
    {

        public AtributiArtiklaRepository(DataAccessContext context) : base(context)
        {
        }
    }
}
