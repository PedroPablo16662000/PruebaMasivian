using Backend.RuletaMasivian.Entities.Interface.Business;
using Backend.RuletaMasivian.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.RuletaMasivian.API.Controllers
{
    /// <summary>
    /// Aprobacion RuletaMasivian.
    /// </summary>
    /// <seealso cref="Base.BaseController" />
    public class AprobacionRuletaMasivianController : Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly IAprobacionRuletaMasivianBusiness _business;
        //cambio para nueva globalización
        /// <summary>
        /// Initializes a new instance of the <see cref="AprobacionRuletaMasivianController"/> class.
        /// </summary>
        /// <param name="business">The business.</param>
        public AprobacionRuletaMasivianController(IAprobacionRuletaMasivianBusiness business)
        {
            _business = business;
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es-ES");
            culture.NumberFormat.CurrencyDecimalSeparator = ".";
            culture.NumberFormat.NumberGroupSeparator = ",";
            culture.NumberFormat.NumberDecimalSeparator = ",";
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Aprobacion RuletaMasivian. Genera lista de todos los RuletaMasivian
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _business.GetAll();
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Aprobacion RuletaMasivian. Búsqueda entre fechas para aprobar
        /// </summary>
        /// <param name="fechaInicial">The fecha inicial.</param>
        /// <param name="fechaFinal">The fecha final.</param>
        /// <param name="tipo">The tipo.</param>
        /// <returns></returns>
        [HttpGet("filtro")]
        public async Task<ActionResult> GetById([FromQuery] DateTime fechaInicial, DateTime fechaFinal, string tipo)
        {
            var result = await _business.GetById(fechaInicial, fechaFinal, tipo);
            return StatusCode(result.Code, result);
        }
        //otro
        /// <summary>
        /// Aprobacion RuletaMasivian. Búsqueda del detalle de un recurso
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpGet("Detalle")]
        public async Task<ActionResult> GetDetalleByIdyTipo([FromQuery] int id, string tipo)
        {
            var result = await _business.GetByIdyTipo(id, tipo);
            return StatusCode(result.Code, result);
        }
        
        /// <summary>
        /// Aprobacion RuletaMasivian. Permite autorizar una lista de RuletaMasivian
        /// </summary>
        /// <param name="listaRuletaMasivian">The lista RuletaMasivian.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Aprobar([FromBody] List<AprobacionRuletaMasivian> listaRuletaMasivian)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es-ES");
            culture.NumberFormat.CurrencyDecimalSeparator = ",";
            culture.NumberFormat.NumberDecimalSeparator = ",";
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            var result = await _business.Aprobar(listaRuletaMasivian, GetEmail, GetUserId);
            return StatusCode(result.Code, Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }
    }
}