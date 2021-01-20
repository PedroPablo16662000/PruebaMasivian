using RuletaPruebaMasivian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuletaPruebaMasivian.Interface.IContext
{
    public interface IRuletaContext
    {
        public bool Exists(int ruleta);

        public bool Open(int ruleta);
        int Add(Ruleta ruleta);
    }
}
