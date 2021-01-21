using RuletaPruebaMasivian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuletaPruebaMasivian.Interface.IContext
{
    public interface IApuestaContext
    {
        int Add(Apuesta apuesta);
    }
}
