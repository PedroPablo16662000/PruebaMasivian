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
            bool response = false;
            response = IsImparBlackPairRed(apuesta);
            response = response ? (apuesta.numero >= 0 && apuesta.numero <= 36) : response;
            response = response ? (apuesta.valorApostado >= 0 && apuesta.valorApostado <= 100000) : response;
            response = response ? HasUserEnoughtMoney(apuesta):response; 

            return response;
        }
        private bool HasUserEnoughtMoney(Apuesta apuesta)
        {
            return _context.HasUserEnoughtMoney(apuesta);
        }
        private bool IsImparBlackPairRed(Apuesta apuesta)
        {
            if ((apuesta.color == "red" && apuesta.numero % 2 == 0) || (apuesta.color == "black" && apuesta.numero % 2 != 0))
                return true;
            else
                return false;
        }
    }
}
