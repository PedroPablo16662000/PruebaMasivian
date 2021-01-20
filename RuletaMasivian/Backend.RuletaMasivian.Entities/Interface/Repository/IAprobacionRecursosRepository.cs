using Backend.RuletaMasivian.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.RuletaMasivian.Entities.Interface.Repositories
{
    public interface IAprobacionRuletaMasivianRepository
    {
        Task<List<AprobacionRuletaMasivian>> GetAll();
        Task<List<bool>> Aprobar(List<AprobacionRuletaMasivian> listaRuletaMasivian, string getEmail, string getUserId);
        Task<List<AprobacionRuletaMasivian>> GetById(DateTime fechaInicial, DateTime fechaFinal, string tipo);
        Task<List<dynamic>> GetDetalle(int id, string tipo);
    }
}
