using System;

namespace RuletaPruebaMasivian.Models
{
    [Serializable]
    public class Ruleta
    {
        public int idRuleta { get; set; }
        public DateTime? fechaInicial { get; set; }
        public DateTime? fechaFinal { get; set; }
        public int dineroPorGanar { get; set; }
        public string marca { get; set; }        
        public string observacion { get; set; }
        public bool estadoActual { get; set; }    
    }
}
