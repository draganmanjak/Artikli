
using AutoMapper;
using DataAccess;
using DataAccess.Infrastructure.Models;
using DataAccess.Infrastructure.ViewModels;
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
    public class AtributiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AtributiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Atributi, AtributiViewModel>().ReverseMap();

            });
            _mapper = config.CreateMapper();
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
            _unitOfWork.Atributis.Add(atribut);
            await _unitOfWork.Complete();
            response.Success = true;
            response.Message = "Uspješno";
            return Ok(response);
        }

        [HttpGet]
        [Route("get-all-atributes")]
        public async Task<IActionResult> GetAllAtributes()
        {
            List<Atributi> atributi = await _unitOfWork.Atributis.GetAll().ToListAsync();
            List<AtributiViewModel> list = _mapper.Map<List<AtributiViewModel>>(atributi);
            foreach (AtributiViewModel atr in list)
            {
                JediniceMjere jedinicaMjere = await 
                 _unitOfWork.JediniceMjeres.Find(x => x.PkJedinicaMjereId == atr.FkJedinicaMjereId).FirstOrDefaultAsync();
                atr.JedinicaMjere = jedinicaMjere?.Naziv;
            }
       


            return Ok(new { list, success=true });
        }
    }
}
