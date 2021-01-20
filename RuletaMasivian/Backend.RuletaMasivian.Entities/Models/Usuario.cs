using System;

namespace Backend.RuletaMasivian.Entities.Models
{
    public partial class Usuario
    {
        public Guid IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Email { get; set; }
        public string CodEstado { get; set; }
        public int IdUsuarioIdentity { get; set; }
        public Guid? IdUsuCrea { get; set; }
        public DateTime? FechaCrea { get; set; }
        public Guid? IdUsuModi { get; set; }
        public DateTime? FechaModi { get; set; }
        public bool? Activo { get; set; }
    }
}
