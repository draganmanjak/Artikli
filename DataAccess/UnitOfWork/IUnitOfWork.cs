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
        IAtributiArtiklaRepository AtributiArtiklas { get; }


        Task<int> Complete();
    }
}
