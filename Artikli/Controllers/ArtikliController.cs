using Artikli.ViewModels;
using Artikli.ViewModels.Models;
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

        public ArtikliController(IMapper mapper, IUnitOfWork unitOfWork, DataAccessContext context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            ServiceResponse<ArtikliViewModel> response = new ServiceResponse<ArtikliViewModel>();
            try
            {
                ArtikliModel artikal = await _context.Artikli.Where(a => a.PkArtikliId == id).FirstOrDefaultAsync();
                if (artikal == null)
                {
                    return BadRequest(new { Message = "Requested item not exists" });
                }
                ArtikliViewModel viewModel = _mapper.Map<ArtikliViewModel>(artikal);

                List<Atributi> atributi = await _context.Atributi.Where(a => a.ArtikliAtributa.Any(aa => aa.PkFkArtikalId == viewModel.PkArtikliId)).ToListAsync();
                viewModel.AtributiArtikla = _mapper.Map<List<AtributiViewModel>>(atributi);

                foreach (AtributiViewModel atribut in viewModel.AtributiArtikla)
                {
                    AtributiArtikla atrArt = await _context.AtributiArtikla.Where(aa => aa.PkFkArtikalId == artikal.PkArtikliId && aa.PkFkAtributId == atribut.PkAtributId).FirstOrDefaultAsync();
                    atribut.Value = atrArt.Value;

                }

                JediniceMjere jedinicaMjere = await _context.JediniceMjere.Where(x => x.PkJedinicaMjereId == viewModel.FkJedinicaMjereId).FirstOrDefaultAsync();
                viewModel.JedinicaMjere = jedinicaMjere?.Naziv;

                response.Model = viewModel;
                response.Success = true;
                response.Message = "Uspješno";
                return Ok(new { response });
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return BadRequest(new { response });
            }

        }
        [HttpPut]
        public async Task<IActionResult> Put(ArtikliViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model is not valid" });
            }
            ServiceResponse<string> response = new ServiceResponse<string>();

            using var transaction = _context.Database.BeginTransaction();
            try
            {

                ArtikliModel artikal = _context.Artikli.Find(model.PkArtikliId);
                artikal.Naziv = model.Naziv;
                artikal.Sifra = model.Sifra;
                artikal.FkJedinicaMjereId = model.FkJedinicaMjereId;
                _context.Artikli.Update(artikal);
                await _context.SaveChangesAsync();
                foreach (var item in model.AtributiArtiklaViewModelList)
                {
                    AtributiArtikla atrArt = _context.AtributiArtikla.Find(model.PkArtikliId, item.PkFkAtributId);
                    if (atrArt != null)
                    {
                        atrArt.Value = item.Value;
                        _context.AtributiArtikla.Update(atrArt);
                    }
                    else
                    {
                        item.PkFkArtikalId = model.PkArtikliId;
                        _context.AtributiArtikla.Add(_mapper.Map<AtributiArtikla>(item));
                    }

                }
                await _context.SaveChangesAsync();

                transaction.Commit();
                response.Success = true;
                response.Message = "Uspješno";
                return Ok(new { response });

            }
            catch (Exception e)
            {
                transaction.Rollback();
                response.Success = false;
                response.Message = e.Message;
                return BadRequest(new { response });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(ArtikliViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model is not valid" });
            }
            ServiceResponse<string> response = new ServiceResponse<string>();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                ArtikliModel artikal = _mapper.Map<ArtikliModel>(model);
                _context.Artikli.Add(artikal);
                await _context.SaveChangesAsync();
                long id = artikal.PkArtikliId;
                foreach (var item in model.AtributiArtiklaViewModelList)
                {
                    item.PkFkArtikalId = id;
                    _context.AtributiArtikla.Add(_mapper.Map<AtributiArtikla>(item));
                }
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Uspješno";
                transaction.Commit();
                return Ok(new { response });

            }
            catch (Exception e)
            {
                transaction.Rollback();
                response.Success = false;
                response.Message = e.Message;
                return BadRequest(new { response });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var artikal = await _context.Artikli.Where(a => a.PkArtikliId == id).FirstOrDefaultAsync();
            ServiceResponse<string> response = new ServiceResponse<string>();
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                if (artikal == null)
                {
                    return BadRequest("Language not exists");
                }
                List<AtributiArtikla> atributiArtikla = await _context.AtributiArtikla.Where(a => a.PkFkArtikalId == artikal.PkArtikliId).ToListAsync();
                foreach (AtributiArtikla atributArtikla in atributiArtikla)
                {
                    _context.AtributiArtikla.Remove(atributArtikla);

                }
                _context.Artikli.Remove(artikal);

                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Uspješno";
                transaction.Commit();
                return Ok(new { response });

            }
            catch (Exception e)
            {
                transaction.Rollback();
                response.Success = false;
                response.Message = e.Message;
                return BadRequest(new { response });
            }

        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetPaginatedArtikli(ArtikliSearchViewModel model)
        {
            ServiceResponse<ArtikliViewModel> response = new ServiceResponse<ArtikliViewModel>();
            try
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
                    artikli = artikli.Where(s => s.AtributiArtikla.Any(aa => aa.PkFkAtributId == item.PkFkAtributId && aa.Value.Contains(item.Value)));
                }
                PaginatedList<ArtikliModel> paginated = await PaginatedList<ArtikliModel>.CreateAsync(artikli, model.PageNum, model.PageSize);
                List<ArtikliViewModel> list = _mapper.Map<List<ArtikliViewModel>>(paginated);

                foreach (ArtikliViewModel art in list)
                {
                    JediniceMjere jedinicaMjere = await _context.JediniceMjere.Where(x => x.PkJedinicaMjereId == art.FkJedinicaMjereId).FirstOrDefaultAsync();
                    art.JedinicaMjere = jedinicaMjere?.Naziv;

                    List<Atributi> atributi = await _context.Atributi.Where(a => a.ArtikliAtributa.Any(aa => aa.PkFkArtikalId == art.PkArtikliId)).ToListAsync();
                    art.AtributiArtikla = _mapper.Map<List<AtributiViewModel>>(atributi);

                    foreach (AtributiViewModel atribut in art.AtributiArtikla)
                    {
                        AtributiArtikla atrArt = await _context.AtributiArtikla.Where(aa => aa.PkFkArtikalId == art.PkArtikliId && aa.PkFkAtributId == atribut.PkAtributId).FirstOrDefaultAsync();
                        atribut.Value = atrArt.Value;

                    }


                }
                return Ok(new { artikli = list, PageNum = paginated.PageIndex, totalPages = artikli.Count() });

            }
            catch (Exception e)
            {

                response.Success = false;
                response.Message = e.Message;
                return BadRequest(response);
            }


        }

    }


}
