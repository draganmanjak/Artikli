
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
    public class ArtikliController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;


        public ArtikliController(IUnitOfWork unitOfWork)
        {
          
            _unitOfWork = unitOfWork;
          
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model is not valid" });
            }
            var response = await _unitOfWork.Artiklis.GetArtikalById(id);

            if (!response.Success)
            {
                return BadRequest(new { response });
            }
            return Ok(new { response });

        }
        [HttpPut]
        public async Task<IActionResult> Put(ArtikliViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model is not valid" });
            }
            var response = await _unitOfWork.Artiklis.UpdateArtikal(model);

            if (!response.Success)
            {
                return BadRequest(new { response });
            }
            return Ok(new { response });
        }
        [HttpPost]
        public async Task<IActionResult> Post(ArtikliViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model is not valid" });
            }
            var response = await _unitOfWork.Artiklis.PostArtikal(model);

            if (!response.Success)
            {
                return BadRequest(new { response });
            }
            return Ok(new { response });

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(long id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model is not valid" });
            }
            var response = await _unitOfWork.Artiklis.DeleteArtikal(id);

            if (!response.Success)
            {
                return BadRequest(new { response });
            }
            return Ok(new { response });
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetPaginatedArtikli(ArtikliSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model is not valid" });
            }
            var response = await _unitOfWork.Artiklis.GetPaginated(model);

            if (!response.Success)
            {
                return BadRequest(new { response });
            }

            return Ok(new { response });
        }

    }


}
