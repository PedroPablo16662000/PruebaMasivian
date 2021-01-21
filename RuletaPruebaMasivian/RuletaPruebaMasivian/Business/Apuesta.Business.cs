using ApuestaPruebaMasivian.Interface.IBusiness;
using RuletaPruebaMasivian.Interface.IContext;
using RuletaPruebaMasivian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApuestaPruebaMasivian.Business
{
    public class ApuestaBusiness : IApuestaBusiness
    {
        private IApuestaContext _context;
        public ApuestaBusiness(IApuestaContext context)
        {
            _context = context;
        }
        public int Add(Apuesta apuesta)
        {
            try
            {
                int id = -1;
                if (Rules(apuesta))
                {
                    id = _context.Add(apuesta);
                    
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool Rules(Apuesta apuesta)
        {
            throw new NotImplementedException();
        }
    }
}
