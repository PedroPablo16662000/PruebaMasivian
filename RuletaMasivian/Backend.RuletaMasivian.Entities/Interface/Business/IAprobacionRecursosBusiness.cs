using Backend.RuletaMasivian.Entities.Models;
using Backend.RuletaMasivian.Entities.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.RuletaMasivian.Entities.Interface.Business
{
    public interface IAprobacionRuletaMasivianBusiness
    {
        Task<ResponseBase<List<AprobacionRuletaMasivian>>> GetAll();
        Task<ResponseBase<List<bool>>> Aprobar(List<AprobacionRuletaMasivian> listaRuletaMasivian, string getEmail, string getUserId);
        Task<ResponseBase<List<AprobacionRuletaMasivian>>> GetById(DateTime fechaInicial, DateTime fechaFinal, string tipo);
        Task<ResponseBase<List<dynamic>>> GetByIdyTipo(int id, string tipo);
    }
}
