using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.RuletaMasivian.Entities.ModelsAdmin
{
    public partial class Rol
    {
        public Guid IdRol { get; set; }
        public string Nombre { get; set; }
        public bool? Estado { get; set; }
        public Guid? IdUsuCrea { get; set; }
        public DateTime? FechaCrea { get; set; }
        public Guid? IdUsuModi { get; set; }
        public DateTime? FechaModi { get; set; }
        public bool? Activo { get; set; }
    }
}
