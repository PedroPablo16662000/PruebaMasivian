using Backend.RuletaMasivian.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.RuletaMasivian.Entities.Interface.Repositories
{
    public interface ICampoRepository
    {
        Task<List<Campo>> GetAll();
        Task<List<Campo>> GetAllByUserId(string user);
        Task<List<TiposCampo>> GetAllTipos();
        Task<List<Proceso>> GetAllProcesos();
        Task<List<Campo>> GetById(int id);
        Task<bool> Create(Campo campo);
        Task<bool> Update(Campo campo);
        Task<bool> Delete();
    }
}
