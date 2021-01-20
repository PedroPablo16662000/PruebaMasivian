using Backend.RuletaMasivian.Entities.Models;
using Backend.RuletaMasivian.Entities.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.RuletaMasivian.Entities.Interface.Business
{
    public interface ICampoBusiness
    {
        Task<ResponseBase<List<Campo>>> GetAll(string user, List<string> rolesIds);
        Task<ResponseBase<List<Campo>>> GetById(int id);
        Task<ResponseBase<bool>> Create(Campo campo);
        Task<ResponseBase<bool>> Update(Campo campo);
        Task<ResponseBase<bool>> Delete();
        Task<ResponseBase<List<TiposCampo>>> GetAllTipos();
        Task<ResponseBase<List<Proceso>>> GetAllProcesos();
        Task<ResponseBase<List<dynamic>>> Aprobacion(List<Campo> campos, string Email);
    }
}
