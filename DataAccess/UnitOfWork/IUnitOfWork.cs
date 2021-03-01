using DataAccess.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
   public interface IUnitOfWork : IDisposable
    {
        IArtikliRepository Artiklis { get; }
        IAtributiRepository Atributis { get; }
        IJediniceMjereRepository JediniceMjeres { get; }


        IAtributiArtiklaRepository AtributiArtiklas { get; }

        Task<int> Complete();
    }
}
