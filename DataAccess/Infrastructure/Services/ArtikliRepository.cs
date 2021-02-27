using DataAccess.Infrastructure.Generic;
using DataAccess.Infrastructure.Interfaces;
using DataAccess.Infrastructure.Models;
using DataAccess.UserManagement.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Infrastructure.Services
{
    public class ArtikliRepository : GenericRepository<ArtikliModel>, IArtikliRepository
    {
       
        public ArtikliRepository(DataAccessContext context) : base(context)
        {
        }


    }
}
