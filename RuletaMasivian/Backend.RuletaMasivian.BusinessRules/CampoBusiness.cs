using Backend.RuletaMasivian.BusinessRules.Middle;
using Backend.RuletaMasivian.Entities.Interface.Business;
using Backend.RuletaMasivian.Entities.Interface.Repositories;
using Backend.RuletaMasivian.Entities.Interface.RepositoryAdmin;
using Backend.RuletaMasivian.Entities.Models;
using Backend.RuletaMasivian.Entities.Responses;
using Backend.RuletaMasivian.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Backend.RuletaMasivian.Entities.Interface.Business;

namespace Backend.RuletaMasivian.BusinessRules
{
    public class CampoBusiness : ICampoBusiness
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly ICampoRepository Repository;

        /// <summary>
        /// The telemetry exception
        /// </summary>
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;

        /// <summary>
        /// Initializes a new instance of the <see cref="CampoBusiness" /> class.
        /// </summary>
        /// <param name="commonIntegrador">The common integrador.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="telemetryException">The telemetry exception.</param>
        /// <param name="sendMailService">The send mail service.</param>
        public CampoBusiness(ICommonIntegrador commonIntegrador,
                             ICampoRepository repository,
                             IRolRepository rolRepository,
                             Utilities.Telemetry.ITelemetryException telemetryException,
                             Utilities.SendMail.ISendMailService sendMailService)
        {
            CommonIntegrador = commonIntegrador;
            Repository = repository;
            _rolRepository = rolRepository;
            TelemetryException = telemetryException;
            SendMailService = sendMailService;
        }

        public async Task<ResponseBase<bool>> Create(Campo campo)
        {
            try
            {
                var data = await Repository.Create(campo);

                if (!data)
                {
                    return new ResponseBase<bool>(HttpStatusCode.BadRequest, Messages.ErrorCreation, data);
                }

                return new ResponseBase<bool>(HttpStatusCode.OK, Messages.Created, data);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                string mensaje = !string.IsNullOrEmpty(exc.InnerException.Message) ? exc.InnerException.Message : Messages.ServerError;
                ManejadorExcepciones.ConversionMensaje(ref mensaje);
                return new ResponseBase<bool>(HttpStatusCode.InternalServerError, mensaje);
            }
        }

        public async Task<ResponseBase<bool>> Delete()
        {
            try
            {
                var data = await Repository.Delete();

                if (!data)
                {
                    return new ResponseBase<bool>(HttpStatusCode.BadRequest, Messages.ErrorCreation, data);
                }

                return new ResponseBase<bool>(HttpStatusCode.OK, Messages.Created, data);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return new ResponseBase<bool>(HttpStatusCode.InternalServerError, exc.Message);
            }
        }

        public async Task<ResponseBase<List<Campo>>> GetAll(string user, List<string> rolesIds)
        {
            try
            {
                List<Campo> data = new List<Campo>();
                var getRoles = await _rolRepository.GetRolesIds(l => rolesIds.Contains(l.IdRol.ToString()));
                if (getRoles.Count(l => l.Nombre.Contains("Administrador_BDP")) > 0)
                    data = await Repository.GetAll();
                else
                    data = await Repository.GetAllByUserId(user);

                return new ResponseBase<List<Campo>>(HttpStatusCode.OK, data.Any() ? Messages.Created : "Ningún ítem encontrado", data, data.Count);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return new ResponseBase<List<Campo>>(HttpStatusCode.InternalServerError, exc.Message);
            }
        }

        public async Task<ResponseBase<List<Campo>>> GetById(int id)
        {
            try
            {
                var data = await Repository.GetById(id);

                return new ResponseBase<List<Campo>>(HttpStatusCode.OK, data.Any() ? Messages.Created : "Ningún ítem encontrado", data, data.Count);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return new ResponseBase<List<Campo>>(HttpStatusCode.InternalServerError, exc.Message);
            }
        }

        public async Task<ResponseBase<bool>> Update(Campo campo)
        {
            try
            {
                if (campo.RequiereCompletamiento == null)
                {
                    campo.RequiereCompletamiento = true;
                    campo.Id = Repository.GetAll().Result.Where(x => x.NombreCampo == campo.NombreCampo).Select(x => x.Id).FirstOrDefault();
                }
                if (campo.IdCampo == "0") campo.Descripcion = "NUEVO";
                var data = await Repository.Update(campo);

                if (!data)
                {
                    return new ResponseBase<bool>(HttpStatusCode.BadRequest, Messages.ErrorCreation, data);
                }

                return new ResponseBase<bool>(HttpStatusCode.OK, Messages.Modified, data);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return new ResponseBase<bool>(HttpStatusCode.InternalServerError, exc.Message);
            }
        }

        public async Task<ResponseBase<List<TiposCampo>>> GetAllTipos()
        {
            try
            {
                var data = await Repository.GetAllTipos();

                return new ResponseBase<List<TiposCampo>>(HttpStatusCode.OK, data.Any() ? Messages.Created : "Ningún ítem encontrado", data, data.Count);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return new ResponseBase<List<TiposCampo>>(HttpStatusCode.InternalServerError, exc.Message);
            }
        }

        public async Task<ResponseBase<List<Proceso>>> GetAllProcesos()
        {
            try
            {
                var data = await Repository.GetAllProcesos();

                return new ResponseBase<List<Proceso>>(HttpStatusCode.OK, data.Any() ? Messages.Created : "Ningún ítem encontrado", data, data.Count);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return new ResponseBase<List<Proceso>>(HttpStatusCode.InternalServerError, exc.Message);
            }
        }

        public async Task<ResponseBase<List<dynamic>>> Aprobacion(List<Campo> campos, string Email)
        {
            var response = new ResponseBase<List<dynamic>>();
            try
            {
                if (campos.Any(a => !a.NombreEstado.HasValue()))
                {
                    throw new DataException("estado de proceso no corresponde");
                }

                if (campos.Any(a => a.NombreEstado.Equals("APROBADO", StringComparison.InvariantCultureIgnoreCase) &&
                                    (
                                        a.Descripcion.Equals("NUEVO", StringComparison.InvariantCultureIgnoreCase) ||
                                        a.Descripcion.Equals("ACTUALIZACIÓN", StringComparison.InvariantCultureIgnoreCase)
                                    )
                               ))
                {
                    throw new DataException("descripción de proceso no corresponde");
                }

                List<element> lstCrea = new List<element>();
                List<element> lstModifica = new List<element>();
                EmailInfo email = new EmailInfo();
                foreach (var item in campos)
                {
                    switch (item.NombreEstado.ToUpper())
                    {
                        case "APROBADO":
                            CampoInfraestructura campoAGuardarBDP = new CampoInfraestructura
                            {
                                CEBE = item.IdCebe,
                                CECO = item.IdCeco,
                                DESCRIPCION_CORTA = item.DescripcionBreve,
                                FECHA_DESCUBRIMIENTO = item.FechaDescubrimiento.Value.ToString("dd-MM-yyyy"),
                                FECHA_EFECTIVIDAD = item.FechaEfectividadCampo.Value.ToString("dd-MM-yyyy"),
                                GERENCIA = item.IdSuperintendenciaCoordinacion,
                                NOMBRE_CAMPO = item.NombreCampo,
                                PROCESO = item.IdNombreProceso,
                                TIPO_CAMPO = item.IdTipoCampo
                            };

                            if (item.Descripcion.Equals("NUEVO", StringComparison.InvariantCultureIgnoreCase))
                            {
                                lstCrea.Add(campoAGuardarBDP);
                            }
                            else
                            {
                                lstModifica.Add(campoAGuardarBDP);
                            }
                            break;
                        case "RECHAZADO":
                            email = new EmailInfo() { To = Email, Subject = "BDP-RuletaMasivian", Body = "Se rechazo la operación con el recurso" };
                            break;
                        case "DEVUELTO":
                            email = new EmailInfo() { To = Email, Subject = "BDP-RuletaMasivian", Body = "Se devolvió la operación con el recurso" };
                            break;
                    }

                    var campo = await Repository.Update(item);
                }

                List<dynamic> listaEnviadaNuevo = new List<dynamic>();
                List<dynamic> listaEnviadaActualizar = new List<dynamic>();
                if (lstCrea.Any())
                {
                    listaEnviadaNuevo = await CommonIntegrador.AutorizarProcesoAsync(lstCrea, "CAMPO_CREA");
                }

                if (lstModifica.Any())
                {
                    listaEnviadaActualizar = await CommonIntegrador.AutorizarProcesoAsync(lstModifica, "CAMPO_ACTUALIZA");
                }

                List<dynamic> listaEnviada = listaEnviadaNuevo.Union(listaEnviadaActualizar).ToList();
                response.Data = listaEnviada;
                if (!listaEnviada.Any())
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Count = 0;
                    response.Message = "Recurso modificado sin autorizar";
                    return response;
                }

                int i = 0;
                foreach (var item in campos)
                {
                    var itemGuardado = listaEnviada[i];
                    var itemSerializado = itemGuardado.Serialize();

                    if (itemSerializado.Contains("M_IERROR\":\"Ok"))
                    {
                        if (itemSerializado.Contains("ID\""))
                        {
                            item.IdCampo = itemSerializado.Split("ID\"", ",");
                        }

                        await Repository.Update(item);
                    }
                    i++;
                }

                email = new EmailInfo() { To = Email, Subject = "BDP-RuletaMasivian", Body = "Se realizo la autorización de los RuletaMasivian" };
                response.Code = (int)HttpStatusCode.OK;
                response.Count = response.Data.Count;
                response.Message = Messages.Created;

                await SendMailService.SendEmailAsync(email);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);

                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Count = 0;
                response.Message = Messages.ServerError;
            }
            return response;
        }
    }
}