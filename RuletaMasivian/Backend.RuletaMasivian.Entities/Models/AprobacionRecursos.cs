using System;

namespace Backend.RuletaMasivian.Entities.Models
{
    public class AprobacionRuletaMasivian
    {
        public string ROW_CREATED_BY { get; set; }
        public DateTime? ROW_CREATED_DATE { get; set; }
        public DateTime? ROW_CHANGED_DATE { get; set; }
        public string ESTADO { get; set; }
        public string NOMBRE_RECURSO { get; set; }
        public string TIPO_RECURSO { get; set; }
        public int? ID { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string OBSERVACION { get; set; }
    }
}
