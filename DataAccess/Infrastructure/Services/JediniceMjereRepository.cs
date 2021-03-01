
using AutoMapper;
using DataAccess.Infrastructure.Generic;
using DataAccess.Infrastructure.Interfaces;
using DataAccess.Infrastructure.Models;
using DataAccess.Infrastructure.ViewModels;
using DataAccess.UserManagement.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructure.Services
{
    public class JediniceMjereRepository : GenericRepository<JediniceMjere>, IJediniceMjereRepository
    {

        public JediniceMjereRepository(DataAccessContext context) : base(context)
        {

        }

    }
}
