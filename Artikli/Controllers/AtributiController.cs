using Artikli.ViewModels.Models;
using AutoMapper;
using DataAccess;
using DataAccess.Infrastructure.Models;
using DataAccess.UserManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artikli.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AtributiController : ControllerBase
    {
        private readonly DataAccessContext _context;
        private readonly IMapper _mapper;

        public AtributiController(DataAccessContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AtributiViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model is not valid" });
            }
            ServiceResponse<string> response = new ServiceResponse<string>();
               Atributi atribut = _mapper.Map<Atributi>(model);
            _context.Atributi.Add(atribut);
            await _context.SaveChangesAsync();
            response.Success = true;
            response.Message = "Uspješno";
            return Ok(response);
        }

        [HttpGet]
        [Route("get-all-atributes")]
        public async Task<IActionResult> GetAllAtributes()
        {
            List<Atributi> atributi = await _context.Atributi.ToListAsync();
          
                List<AtributiViewModel> list = _mapper.Map<List<AtributiViewModel>>(atributi);
            foreach (AtributiViewModel atr in list)
            {
                JediniceMjere jedinicaMjere = await _context.JediniceMjere.Where(x => x.PkJedinicaMjereId == atr.FkJedinicaMjereId).FirstOrDefaultAsync();
                atr.JedinicaMjere = jedinicaMjere?.Naziv;
            }
       


            return Ok(new { list, success=true });
        }
    }
}
