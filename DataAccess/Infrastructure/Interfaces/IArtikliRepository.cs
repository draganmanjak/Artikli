
using DataAccess.Infrastructure.Generic;
using DataAccess.Infrastructure.Models;
using DataAccess.Infrastructure.ViewModels;
using DataAccess.UserManagement.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructure.Interfaces
{
   public interface IArtikliRepository : IGenericRepository<ArtikliModel>
    {
        Task<ServiceResponse<ArtikliViewModel>> GetArtikalById(long id);
        Task<ServiceResponse<string>> UpdateArtikal(ArtikliViewModel model);
        Task<ServiceResponse<string>> PostArtikal(ArtikliViewModel model);
        Task<ServiceResponse<string>> DeleteArtikal(long id);

        Task<ServiceResponse<PaginatedListViewModel>> GetPaginated(ArtikliSearchViewModel model);
    }
}
