using RuletaPruebaMasivian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuletaPruebaMasivian.Interface.IBusiness
{
    public interface IRuletaBusiness
    {
        string OpenRuleta(int ruleta);
        string CloseRuleta(int ruleta);
        int Add(Ruleta ruleta);
    }
}
