using Backend.RuletaMasivian.Entities.Interface.Business;
using Backend.RuletaMasivian.Entities.Interface.Repositories;
using Backend.RuletaMasivian.Entities.Models;
using Backend.RuletaMasivian.Entities.Responses;
using Backend.RuletaMasivian.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Backend.RuletaMasivian.BusinessRules
{
    public class AprobacionRuletaMasivianBusiness : IAprobacionRuletaMasivianBusiness
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IAprobacionRuletaMasivianRepository Repository;

        /// <summary>
        /// The telemetry exception
        /// </summary>
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;

        /// <summary>
        /// Initializes a new instance of the <see cref="AprobacionRuletaMasivianBusiness" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="telemetryException">The telemetry exception.</param>
        public AprobacionRuletaMasivianBusiness(IAprobacionRuletaMasivianRepository repository,
                                          Utilities.Telemetry.ITelemetryException telemetryException)
        {
            Repository = repository;
            TelemetryException = telemetryException;
        }

        public async Task<ResponseBase<List<AprobacionRuletaMasivian>>> GetAll()
        {
            try
            {
                var data = await Repository.GetAll();

                return new ResponseBase<List<AprobacionRuletaMasivian>>(HttpStatusCode.OK, data.Any() ? "Datos consultados correctamente" : "Ningún ítem encontrado", data, data.Count);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return new ResponseBase<List<AprobacionRuletaMasivian>>(HttpStatusCode.InternalServerError, exc.Message);
            }
        }

        public async Task<ResponseBase<List<AprobacionRuletaMasivian>>> GetById(DateTime fechaInicial, DateTime fechaFinal, string tipo)
        {
            try
            {
                var data = await Repository.GetById(fechaInicial, fechaFinal, tipo);

                return new ResponseBase<List<AprobacionRuletaMasivian>>(HttpStatusCode.OK, data.Any() ? "Datos consultados correctamente" : "Ningún ítem encontrado", data, data.Count);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return new ResponseBase<List<AprobacionRuletaMasivian>>(HttpStatusCode.InternalServerError, exc.Message);
            }
        }

        public async Task<ResponseBase<List<dynamic>>> GetByIdyTipo(int id, string tipo)
        {
            try
            {
                var data = await Repository.GetDetalle(id, tipo);

                return new ResponseBase<List<dynamic>>(HttpStatusCode.OK, data.Any() ? "Datos consultados correctamente" : "Ningún ítem encontrado", data, data.Count);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return new ResponseBase<List<dynamic>>(HttpStatusCode.InternalServerError, exc.Message);
            }
        }
        public async Task<ResponseBase<List<bool>>> Aprobar(List<AprobacionRuletaMasivian> listaRuletaMasivian, string getEmail, string getUserId)
        {
            try
            {
                var data = await Repository.Aprobar(listaRuletaMasivian, getEmail, getUserId);

                return new ResponseBase<List<bool>>(HttpStatusCode.OK, data.Any() ? "Estado cambiado correctamente" : "Error en la aprobación del recurso", data, data.Count);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                string mensajeExterno = exc.Message.ToString();
                ManejadorExcepciones.ConversionMensaje(ref mensajeExterno);
                return new ResponseBase<List<bool>>(HttpStatusCode.InternalServerError, mensajeExterno);
            }
        }
    }
}