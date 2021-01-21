using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuletaPruebaMasivian.Models
{
    public class Apuesta
    {
        public int idApuesta { get; set; }
        public int numero { get; set; }
        public string color { get; set; }
        public int valorApostado { get; set; }
        public int idRuleta { get; set; }
        public string idUsuario { get; set; }
        public string idUsuarioGanador { get; set; }
        public DateTime fechaApuesta { get; set; }
    }
}
