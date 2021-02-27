using Artikli.ViewModels;
using AutoMapper;
using DataAccess;
using DataAccess.Infrastructure.Models;
using DataAccess.UnitOfWork;
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
    public class ArtikliController : ControllerBase
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataAccessContext _context;
        private readonly IMapper _mapper;

        public ArtikliController( IMapper mapper, IUnitOfWork unitOfWork, DataAccessContext context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
              _context =context;
        }
        [HttpPost]
        public async Task<IActionResult> Post(ArtikliViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model is not valid" });
            }
            ServiceResponse<string> response = new ServiceResponse<string>();
            ArtikliModel artikal = _mapper.Map<ArtikliModel>(model);
            _context.Artikli.Add(artikal);
            await _context.SaveChangesAsync();
            long id = artikal.PkArtikliId;
            foreach(var item in model.AtributiArtiklaViewModelList)
            {
                item.PkFkArtikalId = id;
                _context.AtributiArtikla.Add(_mapper.Map <AtributiArtikla >(item));
            }
           await _context.SaveChangesAsync();
            response.Success = true;
            response.Message = "Uspješno";
            return Ok(response);
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetPaginatedArtikli(ArtikliSearchViewModel model)
        {
            IQueryable<ArtikliModel> artikli = from s in _context.Artikli select s;
            if (!String.IsNullOrEmpty(model.Naziv))
            {
                artikli = artikli.Where(s => s.Naziv.Contains(model.Naziv));
            }
            if (!String.IsNullOrEmpty(model.Sifra))
            {
                artikli = artikli.Where(s => s.Sifra.Contains(model.Sifra));
            }
            foreach (var item in model.AtributiArtiklaViewModelList)
            {
                artikli = artikli.Where(s => s.AtributiArtikla.Any(aa=> aa.PkFkAtributId==item.PkFkAtributId && aa.Value.Contains(item.Value)));
            }
            PaginatedList<ArtikliModel> paginated = await PaginatedList<ArtikliModel>.CreateAsync(artikli, model.PageNum, model.PageSize);
            List<ArtikliViewModel> list = _mapper.Map<List<ArtikliViewModel>>(paginated);

            foreach (ArtikliViewModel art in list)
            {
                JediniceMjere jedinicaMjere = await _context.JediniceMjere.Where(x => x.PkJedinicaMjereId == art.FkJedinicaMjereId).FirstOrDefaultAsync();
                art.JedinicaMjere = jedinicaMjere?.Naziv;
            }
            return Ok(new { artikli = _mapper.Map<List<ArtikliViewModel>>(paginated), PageNum = paginated.PageIndex, totalPages=artikli.Count() });
        }

    }

    
}
