
using AutoMapper;
using DataAccess;
using DataAccess.Infrastructure.Models;
using DataAccess.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artikli.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JediniceMjereController : ControllerBase
    {
        private readonly DataAccessContext _context;
        private readonly IMapper _mapper;
 
        public JediniceMjereController(DataAccessContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
      
        }

        [HttpGet]
        [Route("get-all-units")]
        public async Task<IActionResult> GetAllUnits()
        {
            List<JediniceMjere> units = await _context.JediniceMjere.ToListAsync();

            return Ok(_mapper.Map<List<JediniceMjereViewModel>>(units));
        }

    }
}
