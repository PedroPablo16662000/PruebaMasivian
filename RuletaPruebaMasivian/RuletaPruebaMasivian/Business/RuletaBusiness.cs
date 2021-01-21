using RuletaPruebaMasivian.Interface.IBusiness;
using RuletaPruebaMasivian.Interface.IContext;
using RuletaPruebaMasivian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuletaPruebaMasivian.Business
{
    public class RuletaBusiness : IRuletaBusiness
    {
        private IRuletaContext _context;
        public RuletaBusiness(IRuletaContext context)
        {
            _context = context;
        }
        public int Add(Ruleta ruleta)
        {
            try
            {
                int id = _context.Add(ruleta);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string OpenRuleta(int ruleta)
        {
            try
            {
                if (_context.Exists(ruleta))
                {
                    return _context.Open(ruleta) ? "Apertura éxitosa" : "Apertura denegada";
                }
                else
                {
                    throw new Exception($"Número de ruleta {ruleta} no encontrado"); ;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string CloseRuleta(int ruleta)
        {
            try
            {
                if (_context.Exists(ruleta))
                {
                    return _context.Close(ruleta) ? "Cierre éxitosp" : "Cierre denegado";
                }
                else
                {
                    throw new Exception($"Número de ruleta {ruleta} no encontrado"); ;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
