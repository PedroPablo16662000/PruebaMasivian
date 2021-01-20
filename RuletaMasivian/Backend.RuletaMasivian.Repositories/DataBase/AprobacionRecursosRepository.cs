using Backend.RuletaMasivian.Entities.Interface.Repositories;
using Backend.RuletaMasivian.Entities.Models;
using Backend.RuletaMasivian.Entities.Responses;
using Backend.RuletaMasivian.Utilities;

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Backend.RuletaMasivian.Repositories.DataBase
{
    public class AprobacionRuletaMasivianRepository : IAprobacionRuletaMasivianRepository
    {
        /// <summary>
        /// Contexto de RuletaMasivian
        /// </summary>
        private readonly RuletaMasivianContext RuletaMasivianContext;
        private readonly IServiceScopeFactory _serviceProvider;

        private readonly EcoOracleContext EcoOracleContext;

        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;

        public AprobacionRuletaMasivianRepository(RuletaMasivianContext RuletaMasivianContext, EcoOracleContext ecoOracleContext, Utilities.Telemetry.ITelemetryException telemetryException, IServiceScopeFactory serviceScopeFactory)
        {
            RuletaMasivianContext = RuletaMasivianContext;
            EcoOracleContext = ecoOracleContext;
            TelemetryException = telemetryException;
            _serviceProvider = serviceScopeFactory;

        }

        public Task<List<AprobacionRuletaMasivian>> GetAll()
        {
            var lstResultado = RuletaMasivianContext.AprobacionRuletaMasivian.Where(x => x.ESTADO.ToUpper() == "EN PROCESO DE APROBACIÓN").OrderBy(x => x.ROW_CREATED_DATE).ToList();
            return Task.Run(() => lstResultado);
        }
        public Task<List<AprobacionRuletaMasivian>> GetById(DateTime fechaInicial, DateTime fechaFinal, string tipo)
        {
            var lstResultado = RuletaMasivianContext.AprobacionRuletaMasivian.Where(x => (x.ROW_CREATED_DATE >= fechaInicial
            && x.ROW_CREATED_DATE <= DateTime.Parse(fechaFinal.ToString("yyyy-MM-dd 23:59:59")) && (x.TIPO_RECURSO == tipo || tipo == null))
            ).OrderBy(x => x.ROW_CREATED_DATE).ToList();
            //|| (x.ROW_CHANGED_DATE >= fechaInicial
            //&& x.ROW_CHANGED_DATE <= DateTime.Parse(fechaFinal.ToString("yyyy-MM-dd 23:59:59")))
            //&& x.ESTADO.ToUpper() == "EN PROCESO DE APROBACIÓN").Where(x => x.TIPO_RECURSO == tipo || tipo == null).OrderBy(x => x.ROW_CREATED_DATE).ToList();
            return Task.Run(() => lstResultado);
        }
        public Task<List<dynamic>> GetDetalle(int id, string tipo)
        {
            List<dynamic> resultado = new List<dynamic>();
            switch (tipo.ToUpper())
            {
                case
                    "CAMPO":
                    resultado.AddRange(RuletaMasivianContext.Campo.Where(x => x.Id == id).ToList());
                    break;
                case
                     "CAMPO_CONTRATO":
                    resultado.AddRange(RuletaMasivianContext.CampoContrato.Where(x => x.Id == id).ToList());
                    break;
                case
                     "CONTRATO":
                    resultado.AddRange(RuletaMasivianContext.Contrato.Where(x => x.Id == id).ToList());
                    break;
                case
                    "FACILIDAD":
                    resultado.AddRange(RuletaMasivianContext.Facilidad.Where(x => x.Id == id).ToList());
                    break;
                case
                    "POZO":
                    resultado.AddRange(RuletaMasivianContext.Pozo.Where(x => x.Id == id).ToList());
                    break;
                case
                    "POZO_YACIMIENTO":
                    resultado.AddRange(RuletaMasivianContext.PozoYacimiento.Where(x => x.Id == id).ToList());
                    break;
                case
                    "YACIMIENTO":
                    resultado.AddRange(RuletaMasivianContext.Yacimiento.Where(x => x.Id == id).ToList());
                    break;
                case
                    "SISTEMA_TRANSPORTE":
                    resultado.AddRange(RuletaMasivianContext.SistemaTransporte.Where(x => x.Id == id).ToList());
                    break;
                case
                    "COMPAÑIA":
                    resultado.AddRange(RuletaMasivianContext.CompaniaOperadora.Where(x => x.Id == id).ToList());
                    break;
                case "SARTA":
                    resultado.AddRange(RuletaMasivianContext.Sarta.Where(x => x.Id == id).ToList());
                    break;
                default:
                    break;
            }

            return Task.Run(() => resultado);
        }
        public async Task<List<bool>> Aprobar(List<AprobacionRuletaMasivian> listaRuletaMasivian, string getEmail, string getUserId)
        {            
            List<bool> respuesta = new List<bool>() { true };

            return await Task.Run(() => respuesta);
        }
        private async Task<List<dynamic>> AprobacionCompaniaAsync(List<CompaniaOperadora> companias, string Email, string getUserId)
        {
            try
            {
                List<element> lstCrea = new List<element>();
                List<element> lstModifica = new List<element>();
                EmailInfo email = new EmailInfo();
                EmailInfo email2 = new EmailInfo();
                foreach (var item in companias)
                {
                    if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "APROBADO")
                    {
                        CompaniaInfraestructura companiaAGuardarBDP = new CompaniaInfraestructura
                        {
                            ID_COMPANIA = item.CompaniaOperadoraId,
                            NOMBRE_COMPANIA = item.BaName,
                            NOMBRE_CORTO = item.BaShortName,
                            NIT = item.BaCode,
                            PAIS = item.PaisId,
                            PROPIETARIO = item.Propiedad == "Y" ? "SI" : "NO",
                            EXISTE_EN_SAP = item.ExisteMaestraSap == "Y" ? "SI" : "NO",
                            OBSERVACIONES = item.Observacion,
                            CATEGORIA = item.Categoria,
                            ID_CATEGORIA = item.IdCategoria,
                            ROW_CHANGED_BY = getUserId,
                            ROW_CREATED_BY = getUserId
                        };
                        if (item.Descripcion.ToUpper() == "NUEVO")
                        {
                            var compania = new CompaniaRepository(RuletaMasivianContext, SendMailService).Update(item);
                            lstCrea.Add(companiaAGuardarBDP);
                        }
                        else if (item.Descripcion.ToUpper() == "ACTUALIZACIÓN")
                        {
                            var compania = new CompaniaRepository(RuletaMasivianContext, SendMailService).Update(item);
                            lstModifica.Add(companiaAGuardarBDP);
                        }
                        else
                        {
                            throw new DataException("descripción de proceso no corresponde");
                        }
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "RECHAZADO")
                    {
                        await SendMailService.SendEmailAsync(email = new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se rechazó la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                   item.BaName, item.Observacion, "Compañía Operadadora")
                        });
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "DEVUELTO")
                    {
                        await SendMailService.SendEmailAsync(email = new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se devolvió la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                  item.BaName, item.Observacion, "Compañía Operadora")
                        });
                    }
                    else
                    {
                        throw new DataException("estado de proceso no corresponde");
                    }
                }
                List<dynamic> listaEnviadaNuevo = new List<dynamic>();
                List<dynamic> listaEnviadaActualizar = new List<dynamic>();
                if (lstCrea.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstCrea, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_crea_compania", xmlRoot).Result;
                }

                if (lstModifica.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstModifica, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_actualiza_compania", xmlRoot).Result;
                }

                List<dynamic> listaEnviada = listaEnviadaNuevo.Union(listaEnviadaActualizar).ToList();
                var response = listaEnviada;
                if (listaEnviada.Count > 0)
                {
                    int i = 0;
                    foreach (var item in companias)
                    {
                        var itemGuardado = listaEnviada[i];
                        string ierror = string.Empty;
                        string id = string.Empty;
                        foreach (JProperty prop in itemGuardado)
                        {
                            if (prop.Name == "M_IERROR" || prop.Name == "ERROR")
                            {
                                ierror = prop.Value.ToString();
                            }
                            if (prop.Name == "ID")
                            {
                                id = prop.Value.ToString();
                            }
                        }
                        if (ierror.ToUpper() == "OK")
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                item.CompaniaOperadoraId = id;
                                item.BusinessAssociate = id;
                            }
                            await SendMailService.SendEmailAsync(
                            email2 = new EmailInfo()
                            {
                                To = item.CorreoElectronico,
                                Subject = "BDP-RuletaMasivian",
                                Body = string.Format("<h3>Se realizó la aprobación del recurso: '{1} - {0}'</h3>",
                                                    item.BaName, "Compañía Operadora")
                            });
                            await EnviarCorreoRecursoMaestroAprobado(itemGuardado);
                        }
                        else
                        {
                            item.NombreEstado = "En proceso de aprobación";
                            await new CompaniaRepository(RuletaMasivianContext, SendMailService).Update(item);
                            string respuestaError = ierror;
                            throw new DataException(respuestaError);
                        }
                        i++;
                    }
                }
                await new CompaniaRepository(RuletaMasivianContext, SendMailService).Update(companias.FirstOrDefault());

                return response;
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                throw new DataException(ex.Message);
            }
        }
        private async Task<List<dynamic>> AprobacionContratosAsync(List<Contrato> contratos, string Email, string getUserId)
        {
            try
            {
                List<element> lstCrea = new List<element>();
                List<element> lstModifica = new List<element>();
                EmailInfo email = new EmailInfo();
                foreach (var item in contratos)
                {
                    if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "APROBADO")
                    {
                        ContratoInfraestructura contratoAGuardarBDP = new ContratoInfraestructura
                        {
                            ID_CONTRATO = item.IdNombreContrato,
                            FIN_CONTRATO = DateTime.Parse(item.FinalizacionContrato.ToString()).ToString("dd-MM-yyyy"),
                            GERENCIA = item.IdJerarquiaAdministrativa.ToString(),
                            ID_OPERADORA = item.IdCompaniaOperadora.ToString(),
                            INICIO_CONTRATO = DateTime.Parse(item.InicioContrato.ToString()).ToString("dd-MM-yyyy"),
                            NOMBRE_CONTRATO = item.NombreContrato,
                            NUMERO_CONTRATO = item.NumeroContrato,
                            TIPO_CONTRATO = string.IsNullOrEmpty(item.IdTipoContrato) ? "" : item.IdTipoContrato.ToString(),
                            TIPO_ENLACE = item.IdTipoEnlace.ToString(),
                            ID_ASOCIADA = item.CompaniasAdministradorasContrato,
                            ROW_CHANGED_BY = getUserId,
                            ROW_CREATED_BY = getUserId
                        };
                        if (item.Descripcion.ToUpper() == "NUEVO")
                        {
                            lstCrea.Add(contratoAGuardarBDP);
                        }
                        else if (item.Descripcion.ToUpper() == "ACTUALIZACIÓN")
                        {
                            lstModifica.Add(contratoAGuardarBDP);
                        }
                        else
                        {
                            throw new DataException("descripción de proceso no corresponde");
                        }
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "RECHAZADO")
                    {
                        var contrato = new ContratoRepository(RuletaMasivianContext, SendMailService).Update(item);
                        await SendMailService.SendEmailAsync(email = new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se rechazó la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                  item.NombreContrato, item.Observacion, "Contrato")
                        });
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "DEVUELTO")
                    {
                        var contrato = new ContratoRepository(RuletaMasivianContext, SendMailService).Update(item);
                        await SendMailService.SendEmailAsync(email = new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se devolvió la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                  item.NombreContrato, item.Observacion, "Contrato")
                        });
                    }
                    else
                    {
                        throw new DataException("estado de proceso no corresponde");
                    }
                }
                List<dynamic> listaEnviadaNuevo = new List<dynamic>();
                List<dynamic> listaEnviadaActualizar = new List<dynamic>();
                if (lstCrea.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstCrea, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_crea_contrato", xmlRoot).Result;
                }

                if (lstModifica.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstModifica, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_actualiza_contrato", xmlRoot).Result;
                }

                List<dynamic> listaEnviada = listaEnviadaNuevo.Union(listaEnviadaActualizar).ToList();
                var response = listaEnviada;
                if (listaEnviada.Count > 0)
                {
                    int i = 0;
                    foreach (var item in contratos)
                    {
                        var itemGuardado = listaEnviada[i];
                        string ierror = string.Empty;
                        string id = string.Empty;
                        foreach (JProperty prop in itemGuardado)
                        {
                            if (prop.Name == "M_IERROR" || prop.Name == "ERROR")
                            {
                                ierror = prop.Value.ToString();
                            }
                            if (prop.Name == "ID")
                            {
                                id = prop.Value.ToString();
                            }
                        }
                        if (ierror.ToUpper() == "OK")
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                item.IdNombreContrato = id;
                            }

                            //envio de correo                
                            await SendMailService.SendEmailAsync(new EmailInfo()
                            {
                                To = item.CorreoElectronico,
                                Subject = "BDP-RuletaMasivian",
                                Body = string.Format("<h3>Se realizó la aprobación del recurso: '{1} - {0}'</h3>",
                                                    item.NombreContrato, "Contrato")
                            });
                            await EnviarCorreoRecursoMaestroAprobado(itemGuardado);
                        }
                        else
                        {
                            item.NombreEstado = "En proceso de aprobación";
                            await new ContratoRepository(RuletaMasivianContext, SendMailService).Update(item);
                            string respuestaError = ierror;
                            throw new DataException(respuestaError);
                        }
                        i++;
                    }
                }
                await new ContratoRepository(RuletaMasivianContext, SendMailService).Update(contratos.FirstOrDefault());

                return response;
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                throw new DataException(ex.Message);
            }
        }
        private async Task<List<dynamic>> AprobacionCamposAsync(List<Campo> campos, string Email, string getUserId)
        {
            try
            {
                List<element> lstCrea = new List<element>();
                List<element> lstModifica = new List<element>();
                EmailInfo email = new EmailInfo();
                foreach (var item in campos)
                {
                    if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "APROBADO")
                    {
                        CampoInfraestructura campoAGuardarBDP = new CampoInfraestructura
                        {
                            CEBE = item.IdCebe,
                            CECO = item.IdCeco,
                            DESCRIPCION_CORTA = item.DescripcionBreve,
                            FECHA_DESCUBRIMIENTO = DateTime.Parse(item.FechaDescubrimiento.ToString()).ToString("dd-MM-yyyy"),
                            FECHA_EFECTIVIDAD = DateTime.Parse(item.FechaEfectividadCampo.ToString()).ToString("dd-MM-yyyy"),
                            FECHA_EXPIRACION = DateTime.Parse(item.FechaExpiracionCampo.ToString()).ToString("dd-MM-yyyy"),
                            GERENCIA = item.IdSuperintendenciaCoordinacion,
                            NOMBRE_CAMPO = item.NombreCampo,
                            PROCESO = item.IdNombreProceso,
                            TIPO_CAMPO = item.IdTipoCampo,
                            MEZCLA_CLASS_SYS_ID = "MATERIAL_OPE",
                            MEZCLA_CLASS_LEVEL_ID = item.IdMezclaCampoProductoSiv,
                            MEZCLA_CLASS_LEVEL_ID_ANT = item.idMezclaCampoProductoSiv_Anterior,
                            CEBE_ANT = item.idCebe_Anterior,
                            CECO_ANT = item.idCeco_Anterior,
                            PROCESO_ANT = item.idNombreProceso_Anterior,
                            ID_CAMPO = item.IdCampo,
                            ID_COMPANIA = item.IdCompania,
                            ROW_CHANGED_BY = getUserId,
                            ROW_CREATED_BY = getUserId
                        };
                        if (item.Descripcion.ToUpper() == "NUEVO")
                        {
                            lstCrea.Add(campoAGuardarBDP);
                        }
                        else if (item.Descripcion.ToUpper() == "ACTUALIZACIÓN")
                        {
                            lstModifica.Add(campoAGuardarBDP);
                        }
                        else
                        {
                            throw new DataException("descripción de proceso no corresponde");
                        }
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "RECHAZADO")
                    {
                        //envio de correo                
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se rechazó la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                   item.NombreCampo, item.Observacion, "Campo")
                        });
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "DEVUELTO")
                    {
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se devolvió la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                    item.NombreCampo, item.Observacion, "Campo")
                        });
                    }
                    else
                    {
                        throw new DataException("estado de proceso no corresponde");
                    }
                    var campo = new CampoRepository(RuletaMasivianContext, SendMailService).Update(item);
                }
                List<dynamic> listaEnviadaNuevo = new List<dynamic>();
                List<dynamic> listaEnviadaActualizar = new List<dynamic>();
                if (lstCrea.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstCrea, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_crea_campo", xmlRoot).Result;
                }

                if (lstModifica.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstModifica, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_actualiza_campo", xmlRoot).Result;
                }

                List<dynamic> listaEnviada = listaEnviadaNuevo.Union(listaEnviadaActualizar).ToList();
                var response = listaEnviada;
                if (listaEnviada.Count > 0)
                {
                    int i = 0;
                    foreach (var item in campos)
                    {
                        var itemGuardado = listaEnviada[i];
                        string ierror = string.Empty;
                        string id = string.Empty;
                        foreach (JProperty prop in itemGuardado)
                        {
                            if (prop.Name == "M_IERROR" || prop.Name == "ERROR")
                            {
                                ierror = prop.Value.ToString();
                            }
                            if (prop.Name == "ID")
                            {
                                id = prop.Value.ToString();
                            }
                        }
                        if (ierror.ToUpper() == "OK")
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                item.IdCampo = id;
                                item.idCebe_Anterior = item.IdCebe;
                                item.idCeco_Anterior = item.IdCeco;
                            }

                            await SendMailService.SendEmailAsync(new EmailInfo()
                            {
                                To = item.CorreoElectronico,
                                Subject = "BDP-RuletaMasivian",
                                Body = string.Format("<h3>Se realizó la aprobación del recurso: '{1} - {0}'</h3>",
                                                    item.NombreCampo, "Campo")
                            });
                            await EnviarCorreoRecursoMaestroAprobado(itemGuardado);
                        }
                        else
                        {
                            item.NombreEstado = "En proceso de aprobación"; ;
                            await new CampoRepository(RuletaMasivianContext, SendMailService).Update(item);
                            string respuestaError = ierror;
                            throw new DataException(respuestaError);
                        }
                        i++;
                        await new CampoRepository(RuletaMasivianContext, SendMailService).Update(item);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                throw new DataException(ex.Message);
            }
        }

        private async Task EnviarCorreoRecursoMaestroAprobado(dynamic itemGuardado)
        {
            string encabezado = string.Format("<h3>Se notifica la aprobación del recurso: </h3><br/>");
            StringBuilder table = new StringBuilder();
            table.Append("<table class=\"info\">");
            foreach (JProperty prop in itemGuardado)
            {
                table.Append($"<tr><td>{prop.Name}</td><td>{prop.Value.ToString()}</td></tr>");
            }
            table.Append("</table>");
            await SendMailService.SendEmailAsync(new EmailInfo()
            {
                To = RuletaMasivianContext.Parametros.Where(x => x.NombreParametro == "_RuletaMasivianMaestrosBDP_Azure").Select(x => x.ParametroVarchar).FirstOrDefault(),
                Subject = "BDP-RuletaMasivian",
                Body = encabezado + table
            });
        }

        private async Task<List<dynamic>> AprobacionCampoContratosAsync(List<CampoContrato> campoContratos, string Email, string getUserId)
        {
            try
            {
                List<element> lstCrea = new List<element>();
                List<element> lstModifica = new List<element>();
                EmailInfo email = new EmailInfo();
                foreach (var item in campoContratos)
                {
                    if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "APROBADO")
                    {
                        CampoContratoInfraestructura campoContratoAGuardarBDP = new CampoContratoInfraestructura
                        {
                            ID_CAMPO_CONTRATO = int.Parse(item.IdCampoContrato ?? "0"),
                            DESCRIPCION = item.DescripcionCampoContrato,
                            FECHA_DESCUBRIMIENTO = DateTime.Parse(item.FechaDescubrimientoCampoContrato.ToString()).ToString("dd-MM-yyyy"),
                            FECHA_EFECTIVIDAD = DateTime.Parse(item.FechaEfectividadCampoContrato.ToString()).ToString("dd-MM-yyyy"),
                            ID_CAMPO = item.IdCampo,
                            ID_CONTRATO = item.IdContrato,
                            id_SOCIO = item.Asociados,
                            LEY_REGALIAS = item.NombreLeyRegalias,
                            MODALIDAD_CAMPO_CONTRATO = item.IdModalidadCampoContrato,
                            NOMBRE_CAMPO = item.NombreCampo,
                            NOMBRE_CAMPO_CONTRATO = item.NombreCampoContrato,
                            NUMERO_CONTRATO = item.IdContrato,
                            PORCENTAJE_BASICA = item.PorcParticipacionBasica.ToString(),
                            PORCENTAJE_INCREMENTAL = item.PorcParticipacionIncremental.ToString(),
                            ID_CLAVE_AMORTIZACION = item.IdClaveAmortizacion,
                            ID_COMPANIA = item.IdCompania,
                            ROW_CHANGED_BY = getUserId,
                            ROW_CREATED_BY = getUserId
                        };
                        if (item.Descripcion.ToUpper() == "NUEVO")
                        {
                            lstCrea.Add(campoContratoAGuardarBDP);
                        }
                        else if (item.Descripcion.ToUpper() == "ACTUALIZACIÓN")
                        {
                            lstModifica.Add(campoContratoAGuardarBDP);
                        }
                        else
                        {
                            throw new DataException("descripción de proceso no corresponde");
                        }
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "RECHAZADO")
                    {
                        //envio de correo                
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se rechazó la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                   item.NombreCampoContrato, item.Observacion, "Campo Contrato")
                        });
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "DEVUELTO")
                    {
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se devolvió la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                   item.NombreCampoContrato, item.Observacion, "Campo Contrato")
                        });
                    }
                    else
                    {
                        throw new DataException("estado de proceso no corresponde");
                    }
                    var campoContrato = new CampoRepository(RuletaMasivianContext, SendMailService).Update(item);
                }
                List<dynamic> listaEnviadaNuevo = new List<dynamic>();
                List<dynamic> listaEnviadaActualizar = new List<dynamic>();
                if (lstCrea.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstCrea, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_crea_campo_contrato", xmlRoot).Result;
                }

                if (lstModifica.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstModifica, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_Actualiza_campo_contrato", xmlRoot).Result;
                }

                List<dynamic> listaEnviada = listaEnviadaNuevo.Union(listaEnviadaActualizar).ToList();
                var response = listaEnviada;
                if (listaEnviada.Count > 0)
                {
                    int i = 0;
                    foreach (var item in campoContratos)
                    {
                        var itemGuardado = listaEnviada[i];
                        string ierror = string.Empty;
                        string id = string.Empty;
                        foreach (JProperty prop in itemGuardado)
                        {
                            if (prop.Name == "M_IERROR" || prop.Name == "ERROR")
                            {
                                ierror = prop.Value.ToString();
                            }
                            if (prop.Name == "ID")
                            {
                                id = prop.Value.ToString();
                            }
                        }
                        if (ierror.ToUpper() == "OK")
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                item.IdCampoContrato = id;
                            }

                            await SendMailService.SendEmailAsync(new EmailInfo()
                            {
                                To = item.CorreoElectronico,
                                Subject = "BDP-RuletaMasivian",
                                Body = string.Format("<h3>Se realizó la aprobación del recurso: '{1} - {0}'</h3>",
                                                    item.NombreCampoContrato, "Campo Contrato")
                            });
                            await EnviarCorreoRecursoMaestroAprobado(itemGuardado);
                        }
                        else
                        {
                            item.NombreEstado = "En proceso de aprobación";
                            await new CampoRepository(RuletaMasivianContext, SendMailService).Update(item);
                            string respuestaError = ierror;
                            throw new DataException(respuestaError);
                        }
                        i++;
                        await new CampoRepository(RuletaMasivianContext, SendMailService).Update(item);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                throw new DataException(ex.Message);
            }
        }
        private void SetSessionGlobalization(OracleConnection aConnection)
        {
            try
            {
                OracleGlobalization info = aConnection.GetSessionInfo();
                System.Globalization.CultureInfo lCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                lCultureInfo.NumberFormat.NumberDecimalSeparator = ".";
                lCultureInfo.NumberFormat.NumberGroupSeparator = ",";
                var ri = new System.Globalization.RegionInfo(lCultureInfo.LCID);

                info.Calendar = lCultureInfo.Calendar.GetType().Name.Replace("Calendar", String.Empty);
                info.Currency = "$"; //ri.CurrencySymbol;
                info.DualCurrency = "$";// ri.CurrencySymbol;
                info.ISOCurrency = "SPAIN";// ri.ISOCurrencySymbol;
                info.DateFormat = "DD/MM/YYYY"; //lCultureInfo.DateTimeFormat.ShortDatePattern + " " + lCultureInfo.DateTimeFormat.ShortTimePattern.Replace("HH", "HH24").Replace("mm", "mi");
                info.DateLanguage = System.Text.RegularExpressions.Regex.Replace(lCultureInfo.EnglishName, @" \(.+\)", String.Empty);
                info.Language = "SPANISH";
                info.NumericCharacters = lCultureInfo.NumberFormat.NumberDecimalSeparator + lCultureInfo.NumberFormat.NumberGroupSeparator;
                info.TimeZone = String.Format("{0}:{1}", TimeZoneInfo.Local.BaseUtcOffset.Hours, TimeZoneInfo.Local.BaseUtcOffset.Minutes);
                aConnection.SetSessionInfo(info);
            }
            catch (OracleException err)
            {
                throw new Exception(err.Message);
            }
        }
        public Task<List<dynamic>> IntegradorBDP(string storeProcedure, string sXml)
        {
            var result = new ResponseBase<string>();
            string resultadoTMP = string.Empty;

            try
            {
                using OracleConnection con = new OracleConnection(EcoOracleContext.ConnectionString);
               
                using (OracleCommand cmd = new OracleCommand(storeProcedure))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 360;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("un_parametro", OracleDbType.Clob).Value = sXml.ToString();
                    cmd.Parameters.Add("un_resultado", OracleDbType.Clob).Direction = ParameterDirection.Output;                    
                    SetSessionGlobalization(con);
                    cmd.ExecuteReader();
                    var tempClob = (OracleClob)cmd.Parameters["un_resultado"].Value;
                    using StreamReader streamreader = new StreamReader(tempClob, Encoding.Unicode);
                    resultadoTMP = streamreader.ReadToEnd();
                    con.Close();
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(resultadoTMP);
                resultadoTMP = JsonConvert.SerializeXmlNode(doc);
                resultadoTMP = resultadoTMP.Replace("\"", "'");
                resultadoTMP = resultadoTMP.Replace("{'root':{", "");
                resultadoTMP = resultadoTMP.Replace("'element':", "");
                resultadoTMP = resultadoTMP.Replace("}}}", "}");

                List<dynamic> _datosRespuesta = new List<dynamic>();
                var _datosRespuestaArray = resultadoTMP.Split("},{");
                foreach (var item in _datosRespuestaArray)
                {
                    var respuestaDeserializada = ((JToken)JsonConvert.DeserializeObject(resultadoTMP));

                    _datosRespuesta.Add(respuestaDeserializada);
                }
                return Task.Run(() => _datosRespuesta);
            }
            catch (Exception exc)
            {
                //envio temporalmente para probar el xml
                SendMailService.SendEmailAsync(new EmailInfo()
                {
                    To = "cesar.giraldo@futlogy.com",
                    Subject = "RuletaMasivian-Error",
                    Body = exc.Message + " | " + "ErrorIn: <pre>" + sXml.ToString() + "</pre> |ErrorOut: " + resultadoTMP
                });
                TelemetryException.RegisterException(exc);
                throw new DataException(resultadoTMP);
            }
        }
        private async Task<List<dynamic>> AprobacionPozosAsync(List<Pozo> pozos, string Email, string getUserId)
        {
            try
            {
                List<element> lstCrea = new List<element>();
                List<element> lstModifica = new List<element>();
                EmailInfo email = new EmailInfo();
                foreach (var item in pozos)
                {
                    if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "APROBADO")
                    {
                        PozoInfraestructura pozoAGuardarBDP = new PozoInfraestructura
                        {
                            AREA = item.IdArea,
                            CAMPO_CONTRATO = item.IdCampoContrato,
                            NOMBRE_CAMPO_CONTRATO = item.NombreCampoContrato,
                            CLASIFICACION_LAHEE = item.IdClasificacionLahee,
                            COORDENADA_X = Math.Round((decimal)item.CoordenadaPlanaSuperficieEx, 4).ToString(),
                            COORDENADA_Y = Math.Round((decimal)item.CoordenadaPlanaSuperficieNy, 4).ToString(),
                            LATITUD = Math.Round((decimal)item.Latitude, 4).ToString(),
                            LONGITUD = Math.Round((decimal)item.Longitude, 4).ToString(),
                            DATUM = item.IdDatum,
                            DEPARTAMENTO = item.IdDepartamento,
                            ESTADO_POZO = item.IdEstadoPozo,
                            FECHA_COMPLETAMIENTO = ((DateTime)item.FechaCompletamiento).ToString("yyyy-MM-dd"),
                            FECHA_INICIO_PERACION = ((DateTime)item.FechaInicioOperacion).ToString("yyyy-MM-dd"),
                            FECHA_PERFORACION = ((DateTime)item.FechaPerforacion).ToString("yyyy-MM-dd"),
                            GERENCIA = item.IdJerarquiaAdministrativa,
                            IDENTIFICADOR_POZO = item.IdentificadorPozo,
                            ID_CAMPO = item.IdCampo,
                            METODO_PRODUCCION = item.IdMetodoProduccion,
                            MUNICIPIO = item.IdMunicipio,
                            NOMBRE_POZO = item.NombrePozo,
                            OPERADOR = item.IdCompaniaOperadora,
                            ORIGEN = item.IdPuntoOrigen,
                            PROFUNDIDAD_VERTICAL = item.ProfundidadPies,
                            TIPO_POZO = item.IdTipoPozo,
                            ROW_CHANGED_BY = getUserId,
                            ROW_CREATED_BY = getUserId
                        };
                        if (item.Descripcion.ToUpper() == "NUEVO")
                        {
                            lstCrea.Add(pozoAGuardarBDP);
                        }
                        else if (item.Descripcion.ToUpper() == "ACTUALIZACIÓN")
                        {
                            lstModifica.Add(pozoAGuardarBDP);
                        }
                        else
                        {
                            throw new DataException("descripción de proceso no corresponde");
                        }
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "RECHAZADO")
                    {
                        var pozo = new PozoRepository(RuletaMasivianContext, EcoOracleContext, SendMailService).Update(item);
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se rechazó la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                 item.NombrePozo, item.Observacion, "Pozo")
                        });
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "DEVUELTO")
                    {
                        var pozo = new PozoRepository(RuletaMasivianContext, EcoOracleContext, SendMailService).Update(item);
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se devolvió la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                 item.NombrePozo, item.Observacion, "Pozo")
                        });
                    }
                    else
                    {
                        throw new DataException("estado de proceso no corresponde");
                    }
                }
                List<dynamic> listaEnviadaNuevo = new List<dynamic>();
                List<dynamic> listaEnviadaActualizar = new List<dynamic>();
                if (lstCrea.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstCrea, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_crea_pozo", xmlRoot).Result;
                }

                if (lstModifica.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstModifica, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_Actualiza_pozo", xmlRoot).Result;
                }

                List<dynamic> listaEnviada = listaEnviadaNuevo.Union(listaEnviadaActualizar).ToList();
                var response = listaEnviada;
                if (listaEnviada.Count > 0)
                {
                    int i = 0;
                    foreach (var item in pozos)
                    {
                        var itemGuardado = listaEnviada[i];
                        string ierror = string.Empty;
                        string id = string.Empty;
                        foreach (JProperty prop in itemGuardado)
                        {
                            if (prop.Name == "M_IERROR" || prop.Name == "ERROR")
                            {
                                ierror = prop.Value.ToString();
                            }
                            if (prop.Name == "ID")
                            {
                                id = prop.Value.ToString();
                            }
                        }
                        if (ierror.ToUpper() == "OK")
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                item.IdPozo = id;
                            }

                            await SendMailService.SendEmailAsync(new EmailInfo()
                            {
                                To = item.CorreoElectronico,
                                Subject = "BDP-RuletaMasivian",
                                Body = string.Format("<h3>Se realizó la aprobación del recurso: '{1} - {0}'</h3>",
                                                    item.NombrePozo, "Pozo")
                            });
                            await EnviarCorreoRecursoMaestroAprobado(itemGuardado);
                        }
                        else
                        {
                            item.NombreEstado = "En proceso de aprobación";
                            await new PozoRepository(RuletaMasivianContext, EcoOracleContext, SendMailService).Update(item);
                            string respuestaError = ierror;
                            throw new DataException(respuestaError);
                        }
                        i++;

                        await new PozoRepository(RuletaMasivianContext, EcoOracleContext, SendMailService).Update(item);
                    }
                }

                //envio de correo                
                await SendMailService.SendEmailAsync(email);
                await new PozoRepository(RuletaMasivianContext, EcoOracleContext, SendMailService).Update(pozos.FirstOrDefault());

                return response;
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                throw new DataException(ex.Message);
            }
        }

        private async Task<List<dynamic>> AprobacionYacimientosAsync(List<Yacimiento> yacimientos, string Email, string getUserId)
        {
            try
            {
                List<element> lstCrea = new List<element>();
                List<element> lstModifica = new List<element>();
                EmailInfo email = new EmailInfo();
                foreach (var item in yacimientos)
                {
                    if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "APROBADO")
                    {
                        YacimientoInfraestructura yacimientoAGuardarBDP = new YacimientoInfraestructura
                        {
                            ID_YACIMIENTO = item.IdYacimiento,
                            ABREVIACION = item.AbreviacionYacimiento,
                            DESCRIPCION_YACIMIENTO = item.DescripcionYacimiento,
                            NOMBRE_CUENCA = item.IdCuenca,
                            NOMBRE_YACIMIENTO = item.NombreYacimiento,
                            OBSERVACIONES = item.Observaciones,
                            TIPO_FALLA = item.TipoFalla,
                            ROW_CHANGED_BY = getUserId,
                            ROW_CREATED_BY = getUserId
                        };
                        if (item.Descripcion.ToUpper() == "NUEVO")
                        {
                            lstCrea.Add(yacimientoAGuardarBDP);
                        }
                        else if (item.Descripcion.ToUpper() == "ACTUALIZACIÓN")
                        {
                            lstModifica.Add(yacimientoAGuardarBDP);
                        }
                        else
                        {
                            throw new DataException("descripción de proceso no corresponde");
                        }
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "RECHAZADO")
                    {
                        //envio de correo                
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se rechazó la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                 item.NombreYacimiento, item.Observacion, "Yacimiento")
                        });
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "DEVUELTO")
                    {
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se devolvió la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                 item.NombreYacimiento, item.Observacion, "Yacimiento")
                        });
                    }
                    else
                    {
                        throw new DataException("estado de proceso no corresponde");
                    }
                    var yacimiento = new YacimientoRepository(RuletaMasivianContext, SendMailService).Update(item);
                }
                List<dynamic> listaEnviadaNuevo = new List<dynamic>();
                List<dynamic> listaEnviadaActualizar = new List<dynamic>();
                if (lstCrea.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstCrea, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_crea_yacimiento", xmlRoot).Result;
                }

                if (lstModifica.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstModifica, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_actualiza_yacimiento", xmlRoot).Result;
                }

                List<dynamic> listaEnviada = listaEnviadaNuevo.Union(listaEnviadaActualizar).ToList();
                var response = listaEnviada;
                if (listaEnviada.Count > 0)
                {
                    int i = 0;
                    foreach (var item in yacimientos)
                    {
                        var itemGuardado = listaEnviada[i];
                        string ierror = string.Empty;
                        string id = string.Empty;
                        foreach (JProperty prop in itemGuardado)
                        {
                            if (prop.Name == "M_IERROR" || prop.Name == "ERROR")
                            {
                                ierror = prop.Value.ToString();
                            }
                            if (prop.Name == "ID")
                            {
                                id = prop.Value.ToString();
                            }
                        }
                        if (ierror.ToUpper() == "OK")
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                item.IdYacimiento = id;
                            }

                            await SendMailService.SendEmailAsync(new EmailInfo()
                            {
                                To = item.CorreoElectronico,
                                Subject = "BDP-RuletaMasivian",
                                Body = string.Format("<h3>Se realizó la aprobación del recurso: '{1} - {0}'</h3>",
                                                    item.NombreYacimiento, "Yacimiento")
                            });
                            await EnviarCorreoRecursoMaestroAprobado(itemGuardado);
                        }
                        else
                        {
                            item.NombreEstado = "En proceso de aprobación";
                            await new YacimientoRepository(RuletaMasivianContext, SendMailService).Update(item);
                            string respuestaError = ierror;
                            throw new DataException(respuestaError);
                        }
                        i++;
                        await new YacimientoRepository(RuletaMasivianContext, SendMailService).Update(item);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                throw new DataException(ex.Message);
            }
        }
        private async Task<List<dynamic>> AprobacionPozoYacimientosAsync(List<PozoYacimiento> pozoYacimientos, string Email, string getUserId)
        {
            try
            {
                List<element> lstCrea = new List<element>();
                List<element> lstModifica = new List<element>();
                EmailInfo email = new EmailInfo();
                foreach (var item in pozoYacimientos)
                {
                    if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "APROBADO")
                    {
                        PozoYacimientoInfraestructura pozoYacimientoAGuardarBDP = new PozoYacimientoInfraestructura
                        {
                            AREA = item.IdArea,
                            ESTADO = item.IdEstadoPozoYacimiento,
                            FACILIDAD_ALMACEN = item.IdFacilidadBombeo.Contains("-") ? item.IdFacilidadBombeo.Split("-")[0] : item.IdFacilidadBombeo,
                            TIPO_FAC_BOMBEO = item.IdFacilidadBombeo.Contains("-") ? item.IdFacilidadBombeo.Split("-")[1] : "",
                            FACILIDAD_RECOLECCION = item.IdFacilidadRecoleccion.Contains("-") ? item.IdFacilidadRecoleccion.Split("-")[0] : item.IdFacilidadRecoleccion,
                            TIPO_FAC_RECOLECCION = item.IdFacilidadRecoleccion.Contains("-") ? item.IdFacilidadRecoleccion.Split("-")[1] : item.IdFacilidadRecoleccion,
                            FACILIDAD_TRATAMIENTO = item.IdFacilidadTratamiento.Contains("-") ? item.IdFacilidadTratamiento.Split("-")[0] : item.IdFacilidadTratamiento,
                            TIPO_FAC_TRATAMIENTO = item.IdFacilidadTratamiento.Contains("-") ? item.IdFacilidadTratamiento.Split("-")[1] : "",
                            FECHA_COMPLETAMIENTO = DateTime.Parse(item.FechaCompletamiento.ToString()).ToString("dd-MM-yyyy"),
                            FECHA_INICIO = DateTime.Parse(item.FechaInicioMetodoOperacion.ToString()).ToString("dd-MM-yyyy"),
                            //FECHA_METODO_OPERACION = DateTime.Parse(item.FechaInicioMetodoOperacion.ToString()).ToString("dd-MM-yyyy"),
                            ID_CAMPO = item.IdCampo,
                            ID_CAMPO_CONTRATO = item.IdCampoContrato,
                            ID_POZO = item.IdPozo,
                            ID_YACIMIENTO = item.IdYacimiento,
                            METODO_PRODUCCION = item.IdMetodoProduccion,
                            NOMBREPOZOYACIMIENTO = item.NombrePozoYacimiento,
                            PRODUCTO_PRIMARIO = item.IdProductoPrimario,
                            SARTA = item.Sarta.ToString(),
                            ID_POZOYACI = item.IdPozoYacimiento,
                            STRAT_UNIT_ID = item.StratUnitId,
                            STRAT_NAME_SET_ID = item.StratNameSetId,
                            ROW_CHANGED_BY = getUserId,
                            ROW_CREATED_BY = getUserId
                        };
                        if (item.Descripcion.ToUpper() == "NUEVO")
                        {
                            lstCrea.Add(pozoYacimientoAGuardarBDP);
                        }
                        else if (item.Descripcion.ToUpper() == "ACTUALIZACIÓN")
                        {
                            lstModifica.Add(pozoYacimientoAGuardarBDP);
                        }
                        else
                        {
                            throw new DataException("descripción de proceso no corresponde");
                        }
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "RECHAZADO")
                    {
                        var pozoYacimiento = new PozoYacimientoRepository(RuletaMasivianContext, SendMailService).Update(item);
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se rechazó la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                 item.NombrePozoYacimiento, item.Observacion, "Pozo-Yacimiento")
                        });
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "DEVUELTO")
                    {
                        var pozoYacimiento = new PozoYacimientoRepository(RuletaMasivianContext, SendMailService).Update(item);
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se devolvió la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                 item.NombrePozoYacimiento, item.Observacion, "Pozo-Yacimiento")
                        });
                    }
                    else
                    {
                        throw new DataException("estado de proceso no corresponde");
                    }
                }
                List<dynamic> listaEnviadaNuevo = new List<dynamic>();
                List<dynamic> listaEnviadaActualizar = new List<dynamic>();
                if (lstCrea.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstCrea, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_crea_pozo_yacimiento", xmlRoot).Result;
                }

                if (lstModifica.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstModifica, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_actualiza_pozo_yacimiento", xmlRoot).Result;
                }

                List<dynamic> listaEnviada = listaEnviadaNuevo.Union(listaEnviadaActualizar).ToList();
                var response = listaEnviada;
                if (listaEnviada.Count > 0)
                {
                    int i = 0;
                    foreach (var item in pozoYacimientos)
                    {
                        var itemGuardado = listaEnviada[i];
                        string ierror = string.Empty;
                        string id = string.Empty;
                        foreach (JProperty prop in itemGuardado)
                        {
                            if (prop.Name == "M_IERROR" || prop.Name == "ERROR")
                            {
                                ierror = prop.Value.ToString();
                            }
                            if (prop.Name == "ID")
                            {
                                id = prop.Value.ToString();
                            }
                        }
                        if (ierror.ToUpper() == "OK")
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                item.IdPozoYacimiento = id;
                            }

                            await SendMailService.SendEmailAsync(new EmailInfo()
                            {
                                To = item.CorreoElectronico,
                                Subject = "BDP-RuletaMasivian",
                                Body = string.Format("<h3>Se realizó la aprobación del recurso: '{1} - {0}'</h3>",
                                                    item.NombrePozoYacimiento, "Pozo-Yacimiento")
                            });
                            await EnviarCorreoRecursoMaestroAprobado(itemGuardado);
                        }
                        else
                        {
                            item.NombreEstado = "En proceso de aprobación";
                            await new PozoYacimientoRepository(RuletaMasivianContext, SendMailService).Update(item);
                            string respuestaError = ierror;
                            throw new DataException(respuestaError);
                        }
                        i++;
                    }
                }
                await new PozoYacimientoRepository(RuletaMasivianContext, SendMailService).Update(pozoYacimientos.FirstOrDefault());

                return response;
            }
            catch (Exception ex)
            {
                string strException = ex.Message.ToString();
                ManejadorExcepciones.ConversionMensaje(ref strException);
                TelemetryException.RegisterException(ex);
                throw new Exception(strException);
            }
        }
        private async Task<List<dynamic>> AprobacionSistemaTransportesAsync(List<SistemaTransporte> sistemaTransportes, string Email, string getUserId)
        {
            var response = new List<dynamic>();
            try
            {
                List<element> lstCrea = new List<element>();
                List<element> lstModifica = new List<element>();
                foreach (var item in sistemaTransportes)
                {
                    if (item.NombreEstado.ToUpper() == "APROBADO")
                    {
                        if (item.Descripcion.ToUpper() == "NUEVO")
                        {
                            item.IdSistemaTransporte = NuevoSistemaTransporteBDP(item);
                            item.NombreEstado = "Aprobado";
                            item.Estado = "Aprobado";
                        }
                        else if (item.Descripcion.ToUpper() == "ACTUALIZACIÓN")
                        {
                            item.NombreEstado = "Aprobado";
                            item.Estado = "Aprobado";
                            ActualizaSistemaTransporteBDP(item);
                        }
                        else
                        {
                            item.NombreEstado = "En proceso de aprobación";
                            item.Estado = "En proceso de aprobación";
                            throw new DataException("descripción de proceso no corresponde");
                        }

                        //envio de correo                
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se realizó la aprobación del recurso: '{1} - {0}'</h3>",
                                                   item.NombreLineaDeTransporte, "Sistema de Transporte")
                        });
                        await EnviarCorreoRecursoMaestroAprobado(item);
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "RECHAZADO")
                    {
                        item.Estado = "RECHAZADO";
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se rechazó la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                 item.NombreLineaDeTransporte, item.Observacion, "Sistema de Transporte")
                        });
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "DEVUELTO")
                    {
                        item.Estado = "DEVUELTO";
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se devolvió la aprobación del recurso: '{1} - {0}', con la observación: {1}</h3>",
                                                 item.NombreLineaDeTransporte, item.Observacion)
                        });
                    }
                    else
                    {
                        throw new DataException("estado de proceso no corresponde");
                    }
                    var sistemaTransporte = new SistemaTransporteRepository(RuletaMasivianContext, EcoOracleContext, SendMailService).Update(item);
                }
                RuletaMasivianContext.SaveChanges();
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                throw new DataException(ex.Message);
            }
            return response;
        }

        private async Task<List<dynamic>> AprobacionSartaAsync(List<Sarta> sarta, string Email)
        {
            var response = new List<dynamic>();
            try
            {
                List<element> lstCrea = new List<element>();
                List<element> lstModifica = new List<element>();
                EmailInfo email = new EmailInfo();
                foreach (var item in sarta)
                {
                    string number = Convert.ToDecimal(Math.Round((decimal)item.Total_Depht,2)).ToString(CultureInfo.CreateSpecificCulture("es-ES"));
                    number = number.Replace(".", ",");
                    if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "APROBADO")
                    {
                        SartaInfraestructura sartaInfraestructura = new SartaInfraestructura
                        {
                            SARTA = item.IdSarta,
                            ID_CAMPO = item.Field_Id,
                            NOMBRE_CAMPO = item.Field_Name,
                            ID_POZO = item.Uwi,
                            NOMBRE_POZO = item.Uwi_Name,
                            ID_YACIMIENTO = item.Strat_Unit_Id,
                            NOMBRE_YACIMIENTO = item.Strat_Name_Set,
                            ID_TIPO_SARTA = item.Prod_String_Type,
                            NOMBRE_TIPO_SARTA = item.Prod_String_Type_Name,
                            FECHA_EFECTIVIDAD = Convert.ToDateTime(item.Effective_Date.ToString()).ToString("yyyyMMdd"),
                            FECHA_EXPIRACION = Convert.ToDateTime(item.Expiry_Date.ToString()).ToString("yyyyMMdd"),
                            NOMBRE_SARTA = item.String_Id,
                            ID_WELLBORE = item.Alias_Id,
                            NOMBRE_WELLBORE = item.Alias_Full_Name,
                            ID_OPERADOR = item.Business_Associate,
                            NOMBRE_OPERADOR = item.Business_Associate_Name,
                            SOURCE = item.Source,
                            STATUS = item.Status,
                            STATUS_TYPE = item.Status_Type,
                            TOTAL_DEPHT = Math.Round((decimal)item.Total_Depht, 2).ToString(),
                            ROW_CREATED_BY = item.Row_Created_By,
                            ROW_CHANGED_BY = item.Row_Changed_By
                        };
                        if (item.Descripcion.ToUpper() == "NUEVO")
                        {
                            lstCrea.Add(sartaInfraestructura);
                        }
                        else if (item.Descripcion.ToUpper() == "ACTUALIZACIÓN")
                        {
                            lstModifica.Add(sartaInfraestructura);
                        }
                        else
                        {
                            throw new DataException("descripción de proceso no corresponde");
                        }
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "RECHAZADO")
                    {
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se rechazó la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                   item.Field_Name, item.Observacion, "Campo")
                        });
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "DEVUELTO")
                    {
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se devolvió la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                    item.Field_Name, item.Observacion, "Campo")
                        });
                    }
                    else
                    {
                        throw new DataException("estado de proceso no corresponde");
                    }
                }
                List<dynamic> listaEnviadaNuevo = new List<dynamic>();
                List<dynamic> listaEnviadaActualizar = new List<dynamic>();
                if (lstCrea.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstCrea, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_crea_Sarta", xmlRoot).Result;
                }

                if (lstModifica.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstModifica, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_actualiza_Sarta", xmlRoot).Result;
                }

                List<dynamic> listaEnviada = listaEnviadaNuevo.Union(listaEnviadaActualizar).ToList();
                response = listaEnviada;
                List<Sarta> updateSarta = new List<Sarta>();
                if (listaEnviada.Count > 0)
                {
                    int i = 0;
                    foreach (var item in sarta)
                    {
                        var itemGuardado = listaEnviada[i];
                        string ierror = string.Empty;
                        string id = string.Empty;
                        foreach (JProperty prop in itemGuardado)
                        {
                            if (prop.Name == "M_IERROR" || prop.Name == "ERROR")
                            {
                                ierror = prop.Value.ToString();
                            }
                            if (prop.Name == "ID")
                            {
                                id = prop.Value.ToString();
                            }
                        }
                        if (ierror.ToUpper() == "OK")
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                item.IdSarta = id;
                            }

                            await SendMailService.SendEmailAsync(new EmailInfo()
                            {
                                To = item.CorreoElectronico,
                                Subject = "BDP-RuletaMasivian",
                                Body = string.Format("<h3>Se realizó la aprobación del recurso: '{1} - {0}'</h3>",
                                                    item.String_Id, "Campo")
                            });
                            await EnviarCorreoRecursoMaestroAprobado(itemGuardado);
                        }
                        else
                        {
                            item.NombreEstado = "En proceso de aprobación";
                            string respuestaError = ierror;
                            throw new DataException(respuestaError);
                        }
                        i++;
                        updateSarta.Add(item);
                    }
                }
                if (updateSarta.Count != 0)
                    await new SartaRepository(_serviceProvider, EcoOracleContext, SendMailService).UpdateSarta(updateSarta.FirstOrDefault());
                else
                    await new SartaRepository(_serviceProvider, EcoOracleContext, SendMailService).UpdateSarta(sarta.FirstOrDefault());

                return response;

            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                throw new DataException(ex.Message);
            }
        }

        private void ActualizaSistemaTransporteBDP(SistemaTransporte st)
        {
            using OracleConnection connection = new OracleConnection(EcoOracleContext.ConnectionString);
            OracleCommand command = new OracleCommand("ADMONFUNC.PKG_MBDP_RuletaMasivian.SISTEMA_TRANSPORTE_UP")
            {
                CommandType = CommandType.StoredProcedure
            };
            OracleParameter oracleParameter1 = new OracleParameter("i_LINEA", OracleDbType.Varchar2);
            OracleParameter oracleParameter2 = new OracleParameter("i_NOMBRE_LINEA", OracleDbType.Varchar2);
            OracleParameter oracleParameter3 = new OracleParameter("i_TIPO_LINEA", OracleDbType.Varchar2);
            OracleParameter oracleParameter4 = new OracleParameter("i_DESCRIPCION", OracleDbType.Varchar2);
            OracleParameter oracleParameter5 = new OracleParameter("i_FECHA", OracleDbType.Date);
            OracleParameter oracleParameter6 = new OracleParameter("i_CAPACIDAD_BOMBEO", OracleDbType.Varchar2);
            OracleParameter oracleParameter7 = new OracleParameter("i_CAPACIDAD_CONTR_BOMBEO", OracleDbType.Varchar2);
            OracleParameter oracleParameter8 = new OracleParameter("i_TIEMPO_REAL_SER", OracleDbType.Varchar2);
            OracleParameter oracleParameter9 = new OracleParameter("i_TIEMPO_FUERA_SER", OracleDbType.Varchar2);
            OracleParameter oracleParameter10 = new OracleParameter("i_ESTACION_INICIAL", OracleDbType.Varchar2);
            OracleParameter oracleParameter11 = new OracleParameter("i_ESTACION_FINAL", OracleDbType.Varchar2);
            OracleParameter oracleParameter12 = new OracleParameter("i_CLASE_SISTEMA", OracleDbType.Varchar2);
            OracleParameter oracleParameter13 = new OracleParameter("i_LLENO_LINEA", OracleDbType.Varchar2);
            OracleParameter oracleParameter14 = new OracleParameter("i_LONGITUD_LINEA", OracleDbType.Varchar2);
            OracleParameter oracleParameter15 = new OracleParameter("i_ID_SISTEMA_TRA", OracleDbType.Varchar2);
            OracleParameter oracleParameter16 = new OracleParameter("i_NOMBRE_SISTEMA_TRA", OracleDbType.Varchar2);
            OracleParameter oracleParameter17 = new OracleParameter("i_SUPERINTENDENCIA", OracleDbType.Varchar2);
            OracleParameter oracleParameter18 = new OracleParameter("i_ROW_CREATED_BY", OracleDbType.Varchar2);
            OracleParameter oracleParameter19 = new OracleParameter("i_ROW_CHANGED_BY", OracleDbType.Varchar2);
            OracleParameter oracleParameter20 = new OracleParameter("i_TRANSPORTADOR_ASOCIADO", OracleDbType.Varchar2);
            OracleParameter oracleParameter21 = new OracleParameter("I_TIPO_SIST_ASOCIADO", OracleDbType.Varchar2);
            OracleParameter oracleParameter22 = new OracleParameter("i_PUNTO_ENTREGA", OracleDbType.Varchar2);
            OracleParameter oracleParameter23 = new OracleParameter("i_estacion_inicial_tipo", OracleDbType.Varchar2);
            OracleParameter oracleParameter24 = new OracleParameter("i_estacion_final_tipo", OracleDbType.Varchar2);
            OracleParameter oracleParameter25 = new OracleParameter("CUR_CODIGO_SIST_TRANSP", OracleDbType.RefCursor);
            OracleParameter oracleParameter26 = new OracleParameter("IID", OracleDbType.Double);
            OracleParameter oracleParameter27 = new OracleParameter("IERROR", OracleDbType.Varchar2);
            oracleParameter1.Value = st.IdLineaDeTransporte;
            oracleParameter2.Value = st.NombreLineaDeTransporte;
            oracleParameter3.Value = "??";
            oracleParameter4.Value = st.DescripcionSistemaTransporte;
            oracleParameter5.Value = st.FechaCreacion;
            oracleParameter6.Value = st.CapacidadBombeo;
            oracleParameter7.Value = st.CapacidadContratada;
            oracleParameter8.Value = st.TiempoRealServicio;
            oracleParameter9.Value = st.TiempoFueraServicio;
            oracleParameter10.Value = st.IdEstacionInicial;
            oracleParameter11.Value = st.IdEstacionFinal;
            oracleParameter12.Value = st.IdClaseSistema;
            oracleParameter13.Value = st.LlenoDeLinea;
            oracleParameter14.Value = st.LongitudDeLinea;
            oracleParameter15.Value = st.IdSistemaTransporte;
            oracleParameter16.Value = st.NombreLineaDeTransporte;
            oracleParameter17.Value = st.IdJerarquiaAdministrativa;
            oracleParameter18.Value = st.RowCreatedBy;
            oracleParameter19.Value = st.RowChangedBy;
            oracleParameter20.Value = st.IdNombreSistemaTransporteAsociado;
            oracleParameter21.Value = st.TipoFacilidad;
            oracleParameter22.Value = st.IdPuntoDeEntrega;
            oracleParameter23.Value = st.TipoFacilidadEstacionInicial;
            oracleParameter24.Value = st.TipoFacilidadEstacionFinal;
            oracleParameter25.Direction = ParameterDirection.Output;
            oracleParameter26.Value = 0;
            oracleParameter27.Value = "Ok";
            OracleParameter[] oracleParameters = { oracleParameter1, oracleParameter2, oracleParameter3, oracleParameter4,
            oracleParameter5, oracleParameter6, oracleParameter7, oracleParameter8,oracleParameter9, oracleParameter10, oracleParameter11, oracleParameter12,
            oracleParameter13, oracleParameter14, oracleParameter15, oracleParameter16,oracleParameter17, oracleParameter18, oracleParameter19, oracleParameter20,
            oracleParameter21, oracleParameter22, oracleParameter23, oracleParameter24, oracleParameter25, oracleParameter26, oracleParameter27
            };
            command.Parameters.AddRange(oracleParameters);

            command.Connection = connection;
            connection.Open();

            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter();
            var respuesta = command.ExecuteNonQuery();
            connection.Close();
        }

        private string NuevoSistemaTransporteBDP(SistemaTransporte st)
        {
            using OracleConnection connection = new OracleConnection(EcoOracleContext.ConnectionString);
            OracleCommand command = new OracleCommand("ADMONFUNC.PKG_MBDP_RuletaMasivian.SISTEMA_TRANSPORTE_NW")
            {
                CommandType = CommandType.StoredProcedure
            };
            OracleParameter oracleParameter1 = new OracleParameter("i_LINEA", OracleDbType.Varchar2);
            OracleParameter oracleParameter2 = new OracleParameter("i_NOMBRE_LINEA", OracleDbType.Varchar2);
            OracleParameter oracleParameter4 = new OracleParameter("i_DESCRIPCION", OracleDbType.Varchar2);
            OracleParameter oracleParameter5 = new OracleParameter("i_FECHA", OracleDbType.Date);
            OracleParameter oracleParameter6 = new OracleParameter("i_CAPACIDAD_BOMBEO", OracleDbType.Double);
            OracleParameter oracleParameter7 = new OracleParameter("i_CAPACIDAD_CONTR_BOMBEO", OracleDbType.Double);
            OracleParameter oracleParameter8 = new OracleParameter("i_TIEMPO_REAL_SER", OracleDbType.Double);
            OracleParameter oracleParameter9 = new OracleParameter("i_TIEMPO_FUERA_SER", OracleDbType.Double);
            OracleParameter oracleParameter10 = new OracleParameter("i_ESTACION_INICIAL", OracleDbType.Varchar2);
            OracleParameter oracleParameter11 = new OracleParameter("i_ESTACION_FINAL", OracleDbType.Varchar2);
            OracleParameter oracleParameter12 = new OracleParameter("i_CLASE_SISTEMA", OracleDbType.Varchar2);
            OracleParameter oracleParameter13 = new OracleParameter("i_LLENO_LINEA", OracleDbType.Double);
            OracleParameter oracleParameter14 = new OracleParameter("i_LONGITUD_LINEA", OracleDbType.Double);
            //OracleParameter oracleParameter15 = new OracleParameter("i_ID_SISTEMA_TRA", OracleDbType.Varchar2);
            OracleParameter oracleParameter16 = new OracleParameter("i_NOMBRE_SISTEMA_TRA", OracleDbType.Varchar2);
            OracleParameter oracleParameter17 = new OracleParameter("i_SUPERINTENDENCIA", OracleDbType.Varchar2);
            OracleParameter oracleParameter18 = new OracleParameter("i_ROW_CREATED_BY", OracleDbType.Varchar2);
            OracleParameter oracleParameter20 = new OracleParameter("i_TRANSPORTADOR_ASOCIADO", OracleDbType.Varchar2);
            OracleParameter oracleParameter21 = new OracleParameter("I_TIPO_SIST_ASOCIADO", OracleDbType.Varchar2);
            OracleParameter oracleParameter22 = new OracleParameter("i_PUNTO_ENTREGA", OracleDbType.Varchar2);
            OracleParameter oracleParameter23 = new OracleParameter("i_estacion_inicial_tipo", OracleDbType.Varchar2);
            OracleParameter oracleParameter24 = new OracleParameter("i_estacion_final_tipo", OracleDbType.Varchar2);
            OracleParameter oracleParameter25 = new OracleParameter("CUR_CODIGO_SIST_TRANSP", OracleDbType.RefCursor);
            OracleParameter oracleParameter26 = new OracleParameter("IID", OracleDbType.Double);
            OracleParameter oracleParameter27 = new OracleParameter("IERROR", OracleDbType.Varchar2);
            oracleParameter1.Value = st.IdLineaDeTransporte;/// aquí lo que genera en código de clase de sistema
            oracleParameter2.Value = st.NombreLineaDeTransporte;
            oracleParameter4.Value = st.DescripcionSistemaTransporte;
            oracleParameter5.Value = (DateTime)(st.FechaCreacion);
            oracleParameter6.Value = st.CapacidadBombeo;
            oracleParameter7.Value = st.CapacidadContratada;
            oracleParameter8.Value = st.TiempoRealServicio;
            oracleParameter9.Value = st.TiempoFueraServicio;
            oracleParameter10.Value = st.IdEstacionInicial;
            oracleParameter11.Value = st.IdEstacionFinal;
            oracleParameter12.Value = st.IdClaseSistema;// lo que genera en tipo de la lista clase de sistema
            oracleParameter13.Value = st.LlenoDeLinea;
            oracleParameter14.Value = st.LongitudDeLinea;
            //oracleParameter15.Value = "??";
            oracleParameter16.Value = st.NombreLineaDeTransporte;
            oracleParameter17.Value = st.IdJerarquiaAdministrativa;
            oracleParameter18.Value = st.RowCreatedBy;
            oracleParameter20.Value = st.IdNombreSistemaTransporteAsociado;
            oracleParameter21.Value = st.TipoFacilidad;
            oracleParameter22.Value = st.IdPuntoDeEntrega;
            oracleParameter23.Value = st.TipoFacilidadEstacionInicial;
            oracleParameter24.Value = st.TipoFacilidadEstacionFinal;
            //oracleParameter23.Direction = ParameterDirection.Output;
            oracleParameter25.Direction = ParameterDirection.Output;
            oracleParameter25.Value = 0F;
            oracleParameter26.Value = "Ok";
            OracleParameter[] oracleParameters = { oracleParameter1, oracleParameter2, oracleParameter4,
            oracleParameter5, oracleParameter6, oracleParameter7, oracleParameter8,oracleParameter9, oracleParameter10, oracleParameter11, oracleParameter12,
            oracleParameter13, oracleParameter14, oracleParameter16,oracleParameter17, oracleParameter18,  oracleParameter20,
            oracleParameter21, oracleParameter22, oracleParameter23, oracleParameter24, oracleParameter25, oracleParameter26,oracleParameter27
            };
            command.Parameters.AddRange(oracleParameters);

            command.Connection = connection;
            connection.Open();

            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter();
            command.ExecuteNonQuery();
            string respuesta = oracleParameter24.Value.ToString();
            connection.Close();
            return respuesta;
        }

        private async Task<List<dynamic>> AprobacionFacilidadsAsync(List<Facilidad> facilidades, string Email, string getUserId)
        {
            try
            {
                List<element> lstCrea = new List<element>();
                List<element> lstModifica = new List<element>();
                EmailInfo email = new EmailInfo();
                foreach (var item in facilidades)
                {
                    if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "APROBADO")
                    {
                        FacilidadInfraestructura facilidadAGuardarBDP = new FacilidadInfraestructura
                        {
                            CAPACIDAD_ALMACENAMIENTO = Math.Round((decimal)item.CapacidadDeAlmacenamiento, 2).ToString(),
                            CONTRATO_ANH = item.NombreContrato,
                            DEPARTAMENTO = item.IdDepartamento,
                            EMPRESA_OPERADORA = item.IdEmpresaOperadora,
                            ESTADO_FACILIDAD = item.IdEstadoFacilidad,
                            FACILIDAD_FISCALIZADORA = item.FacilidadFiscalizadora == true ? "SI" : "NO",
                            GERENCIA = item.IdAreaDministrativa,
                            MEZCLA_PRODUCCION = item.IdMezclaProducccionSiv,
                            MUNICIPIO = item.IdMunicipio,
                            NOMBRE = item.NombreFacilidad,
                            NOMBRE_CAMPO = item.IdCampo,
                            NOMBRE_PROCESO = item.IdProceso,
                            REQUERIDO_SINOPER = item.RequeridoPorSinoper.ToString(),
                            RESPONSABLE_APROBACION = item.IdResponsableAprobacion,//pendiente debe enviar el id del responsable
                            TIPO_FACILIDAD = item.IdTipoFacilidad,
                            TIPO_RESPONSABLE = item.Tiporesponsable,
                            ID_ALMACEN_LOGISTICO = item.IdAlmacenLogistico,
                            AREA_ADMINISTRATIVA = item.IdAreaDministrativa,
                            ID_FACILIDAD = item.IdFacilidad,
                            TIPO_ESTADO = item.TipoEstado,
                            ROW_CHANGED_BY = getUserId,
                            ROW_CREATED_BY = getUserId
                        };
                        if (item.Descripcion.ToUpper() == "NUEVO")
                        {
                            lstCrea.Add(facilidadAGuardarBDP);
                        }
                        else if (item.Descripcion.ToUpper() == "ACTUALIZACIÓN")
                        {
                            lstModifica.Add(facilidadAGuardarBDP);
                        }
                        else
                        {
                            throw new DataException("descripción de proceso no corresponde");
                        }
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "RECHAZADO")
                    {
                        var facilidad = new FacilidadRepository(RuletaMasivianContext, SendMailService).Update(item);
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se rechazó la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                  item.NombreFacilidad, item.Observacion, "Facilidad")
                        });
                    }
                    else if (item.NombreEstado != null && item.NombreEstado.ToUpper() == "DEVUELTO")
                    {
                        var facilidad = new FacilidadRepository(RuletaMasivianContext, SendMailService).Update(item);
                        await SendMailService.SendEmailAsync(new EmailInfo()
                        {
                            To = Email,
                            Subject = "BDP-RuletaMasivian",
                            Body = string.Format("<h3>Se devolvió la aprobación del recurso: '{2} - {0}', con la observación: {1}</h3>",
                                                  item.NombreFacilidad, item.Observacion, "Facilidad")
                        });
                    }
                    else
                    {
                        throw new DataException("estado de proceso no corresponde");
                    }
                }
                List<dynamic> listaEnviadaNuevo = new List<dynamic>();
                List<dynamic> listaEnviadaActualizar = new List<dynamic>();
                if (lstCrea.Count > 0)
                {
                    //listaEnviadaNuevo = await CommonIntegrador.AutorizarProcesoAsync(lstCrea, "FACILIDAD_CREA");
                    var xmlRoot = ConvertTypes.ToXmlString(lstCrea, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_crea_facilidad", xmlRoot).Result;
                }
                if (lstModifica.Count > 0)
                {
                    var xmlRoot = ConvertTypes.ToXmlString(lstModifica, "element", "root");
                    listaEnviadaNuevo = IntegradorBDP("ADMONFUNC.PKG_MBDP_XRESOURCE.p_actualiza_facilidad", xmlRoot).Result;
                    //listaEnviadaActualizar = await CommonIntegrador.AutorizarProcesoAsync(lstModifica, "FACILIDAD_ACTUALIZA");
                }

                List<dynamic> listaEnviada = listaEnviadaNuevo.Union(listaEnviadaActualizar).ToList();
                var response = listaEnviada;
                if (listaEnviada.Count > 0)
                {
                    int i = 0;
                    foreach (var item in facilidades)
                    {
                        var itemGuardado = listaEnviada[i];
                        string ierror = string.Empty;
                        string id = string.Empty;
                        foreach (JProperty prop in itemGuardado)
                        {
                            if (prop.Name == "M_IERROR" || prop.Name == "ERROR")
                            {
                                ierror = prop.Value.ToString();
                            }
                            if (prop.Name == "ID")
                            {
                                id = prop.Value.ToString();
                            }
                        }
                        if (ierror.ToUpper() == "OK")
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                item.IdFacilidad = id;
                            }

                            await SendMailService.SendEmailAsync(new EmailInfo()
                            {
                                To = item.CorreoElectronico,
                                Subject = "BDP-RuletaMasivian",
                                Body = string.Format("<h3>Se realizó la aprobación del recurso: '{1} - {0}'</h3>",
                                                    item.NombreFacilidad, "Facilidad")
                            });
                            await EnviarCorreoRecursoMaestroAprobado(itemGuardado);
                        }
                        else
                        {
                            item.NombreEstado = "En proceso de aprobación";
                            await new FacilidadRepository(RuletaMasivianContext, SendMailService).Update(item);
                            string respuestaError = ierror;
                            throw new DataException(respuestaError);
                        }
                        i++;

                        await new FacilidadRepository(RuletaMasivianContext, SendMailService).Update(item);
                    }
                }

                //envio de correo                
                await new FacilidadRepository(RuletaMasivianContext, SendMailService).Update(facilidades.FirstOrDefault());

                return response;
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                throw new DataException(ex.Message);
            }
        }
    }
}