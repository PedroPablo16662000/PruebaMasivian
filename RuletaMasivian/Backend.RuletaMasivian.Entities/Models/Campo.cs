using System;

namespace Backend.RuletaMasivian.Entities.Models
{
    public partial class Campo
    {

        public int Id { get; set; }
        public string IdCampo { get; set; }
        public string NombreCampo { get; set; }
        public string DescripcionBreve { get; set; }
        public string Descripcion { get; set; }
        public string IdTipoCampo { get; set; }
        public string NombreTipoCampo { get; set; }
        public DateTime? FechaDescubrimiento { get; set; }
        public DateTime? FechaExpiracionCampo { get; set; }
        public DateTime? FechaEfectividadCampo { get; set; }
        public string IdSuperintendenciaCoordinacion { get; set; }
        public string NombreSuperintendenciaCoordinacion { get; set; }
        public string IdMezclaCampoProductoSiv { get; set; }
        public string NombreMezclaCampoProductoSiv { get; set; }
        public string IdCebe { get; set; }
        public string NombreCebe { get; set; }
        public string IdCeco { get; set; }
        public string NombreCeco { get; set; }
        public string IdNombreProceso { get; set; }
        public string NombreProceso { get; set; }
        public int? Estado { get; set; }
        public string RowChangedBy { get; set; }
        public DateTime? RowChangedDate { get; set; }
        public string RowCreatedBy { get; set; }
        public DateTime? RowCreatedDate { get; set; }
        public string NombreEstado { get; set; }
        public string Observacion { get; set; }
        public string CorreoElectronico { get; set; }

        public string idNombreProceso_Anterior { get; set; }
        public string idMezclaCampoProductoSiv_Anterior { get; set; }
        public string idCebe_Anterior { get; set; }
        public string idCeco_Anterior { get; set; }
        public string NombreCompania { get; set; }
        public string IdCompania { get; set; }
        public bool? RequiereCompletamiento { get; set; }
    }
}
