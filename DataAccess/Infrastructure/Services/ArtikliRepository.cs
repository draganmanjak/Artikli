
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
    public class ArtikliRepository : GenericRepository<ArtikliModel>, IArtikliRepository
    {
        private IMapper _mapper;
        public ArtikliRepository(DataAccessContext context) : base(context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ArtikliViewModel, ArtikliModel>().ReverseMap();
                cfg.CreateMap<AtributiArtikla, AtributiArtiklaViewModel>().ReverseMap();
                cfg.CreateMap<Atributi, AtributiViewModel>().ReverseMap();

            });
            _mapper = config.CreateMapper();
        }

        public async Task<ServiceResponse<ArtikliViewModel>> GetArtikalById(long id)
        {

            ServiceResponse<ArtikliViewModel> response = new ServiceResponse<ArtikliViewModel>();
            try
            {
                ArtikliModel artikal = await _context.Artikli.Where(a => a.PkArtikliId == id).FirstOrDefaultAsync();
                if (artikal == null)
                {
                    response.Success = false;
                    response.Message = "Bad request";
                    return response;
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
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return response;
            }

        }

        public async Task<ServiceResponse<string>> PostArtikal(ArtikliViewModel model)
        {
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
                return response;

            }
            catch (Exception e)
            {
                transaction.Rollback();
                response.Success = false;
                response.Message = e.Message;
                return response;
            }
        }

        public async Task<ServiceResponse<string>> UpdateArtikal(ArtikliViewModel model)
        {
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
                return response;

            }
            catch (Exception e)
            {
                transaction.Rollback();
                response.Success = false;
                response.Message = e.Message;
                return response;
            }
        }
        public async Task<ServiceResponse<string>> DeleteArtikal(long id)
        {
            var artikal = await _context.Artikli.Where(a => a.PkArtikliId == id).FirstOrDefaultAsync();
            ServiceResponse<string> response = new ServiceResponse<string>();
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                if (artikal == null)
                {
                    response.Success = false;
                    response.Message = "Artikal ne postoji";
                    return response;
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
                return response;

            }
            catch (Exception e)
            {
                transaction.Rollback();
                response.Success = false;
                response.Message = e.Message;
                return response;
            }
        }

        public async Task<ServiceResponse<PaginatedListViewModel>> GetPaginated(ArtikliSearchViewModel model)
        {
            ServiceResponse<PaginatedListViewModel> response = new ServiceResponse<PaginatedListViewModel>();
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
                PaginatedListViewModel paginatedList = new PaginatedListViewModel();
                paginatedList.Artikli = list;
                paginatedList.PageNum = paginated.PageIndex;
                paginatedList.TotalPages = artikli.Count();
                response.Model = paginatedList;
                response.Success = true;
                response.Message = "Uspjesno";
                return response;

            }
            catch (Exception e)
            {

                response.Success = false;
                response.Message = e.Message;
                return response;
            }
        }
    }
}
