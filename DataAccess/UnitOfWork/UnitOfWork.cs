using AutoMapper;
using DataAccess.Infrastructure.Interfaces;
using DataAccess.Infrastructure.Services;
using DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces.UnitOfWork
{
   public class UnitOfWork: IUnitOfWork
    {
        private readonly DataAccessContext _context;
        public UnitOfWork(DataAccessContext context)
        {
            _context = context;
            Artiklis = new ArtikliRepository(_context);
            AtributiArtiklas = new AtributiArtiklaRepository(_context);
            Atributis = new AtributiRepository(_context);
            JediniceMjeres = new JediniceMjereRepository(_context);

        }

        public IArtikliRepository Artiklis { get; private set; }
        public IAtributiArtiklaRepository AtributiArtiklas { get; private set; }

        public IAtributiRepository Atributis { get; private set; }

        public IJediniceMjereRepository JediniceMjeres { get; private set; }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
