using Backend.RuletaMasivian.Entities.Interface.Repositories;
using Backend.RuletaMasivian.Entities.Models;


using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.RuletaMasivian.Repositories.DataBase
{
    public class CampoRepository : ICampoRepository
    {
        private readonly RuletaMasivianContext RuletaMasivianContext;

        private readonly ISendMailService _sendMailService;

        public CampoRepository(RuletaMasivianContext RuletaMasivianContext, ISendMailService sendMailService)
        {
            RuletaMasivianContext = RuletaMasivianContext;
            _sendMailService = sendMailService;
        }

        public async Task<bool> Create(Campo campoContrato)
        {
            if (campoContrato.Descripcion == "ACTUALIZACIÓN")
            {
                if (campoContrato.IdCampo == 0.ToString())
                {
                    campoContrato.Descripcion = "NUEVO";
                }
                var itemGuardado = RuletaMasivianContext.Campo.Where(x => x.IdCampo == campoContrato.IdCampo).Select(x => x.Id).FirstOrDefault();
                campoContrato.Id = itemGuardado;
                if (campoContrato.Id == 0)
                {
                    campoContrato.NombreEstado = "En proceso de aprobación";
                    RuletaMasivianContext.Campo.Add(campoContrato);
                }
                else
                {
                    await Update(campoContrato);
                }
            }
            else
            {
                campoContrato.NombreEstado = "En proceso de aprobación";
                RuletaMasivianContext.Campo.Add(campoContrato);
            }
            RuletaMasivianContext.SaveChanges();
            string rutaAprobacion = RuletaMasivianContext.Parametros.Where(x => x.NombreParametro == "rutaAprobador").Select(x => x.ParametroVarchar).FirstOrDefault();
            await _sendMailService.SendEmailAsync(new EmailInfo()
            {
                Body = string.Format("<h3>Se notifica el recurso 'Campo - {0}', en estado: {1}, <br/> si corresponde, <a href='{2}'>ingrese para su aprobación</h3></a>",
                campoContrato.NombreCampo, campoContrato.NombreEstado, rutaAprobacion),
                Subject = string.Format("Recurso {0} desde Integrador Volúmetrico BDP", campoContrato.Descripcion),
                To = RuletaMasivianContext.Parametros.Where(x => x.NombreParametro == "correoAprobador").Select(x => x.ParametroVarchar).FirstOrDefault()
            });
            await EnviarCorreoRuletaMasivianMaestrosAsync(campoContrato, _sendMailService);
            return await Task.Run(() => true);
        }
        private async Task EnviarCorreoRuletaMasivianMaestrosAsync(Campo campoContrato, ISendMailService _sendMailService)
        {
            //se crea este formato para enviar por solicitud de Alwin Gómez
            string encabezado = string.Format("<h3>Se notifica el recurso {0}, en proceso de {1} </h3><br/>", campoContrato.NombreCampo, campoContrato.NombreEstado);
            StringBuilder table = new StringBuilder();
            table.Append("<table class=\"info\">");
            table.Append($"<tr><td>Nombre</td><td>{campoContrato.NombreCampo}</td></tr>");
            table.Append($"<tr><td>Contrato</td><td>{campoContrato.NombreContrato}</td></tr>");
            table.Append($"<tr><td>Campo Asociado</td><td>{campoContrato.NombreCampo}</td></tr>");
            table.Append($"<tr><td>Descripción</td><td>{campoContrato.DescripcionCampo}</td></tr>");
            table.Append($"<tr><td>Modalidad</td><td>{campoContrato.NombreModalidadCampo}</td></tr>");
            table.Append($"<tr><td>Fecha Descubrimiento</td><td>{campoContrato.FechaDescubrimientoCampo}</td></tr>");
            table.Append($"<tr><td>Fecha Efectividad</td><td>{campoContrato.FechaEfectividadCampo}</td></tr>");
            table.Append($"<tr><td>Ley de Regalías/Producto SIV</td><td>{campoContrato.NombreLeyRegalias}</td></tr>");
            table.Append($"<tr><td>Compania Socios</td><td>{campoContrato.NombreAsociados}</td></tr>");
            table.Append($"<tr><td>Compania Compañía Operadora</td><td>{campoContrato.NombreCompania}</td></tr>");
            table.Append($"<tr><td>Compania Clave Amortización</td><td>{campoContrato.ClaveDeAmortizacion}</td></tr>");
            table.Append($"<tr><td>Estado</td><td>{campoContrato.NombreEstado}</td></tr>");
            table.Append("</table>");
            await _sendMailService.SendEmailAsync(new EmailInfo()
            {
                Body = encabezado + table.ToString(),
                Subject = string.Format("Recurso {0} desde Integrador Volúmetrico BDP", campoContrato.Descripcion),
                To = RuletaMasivianContext.Parametros.Where(x => x.NombreParametro == "_RuletaMasivianMaestrosBDP_Azure").Select(x => x.ParametroVarchar).FirstOrDefault()
            });
        }
        public Task<bool> Delete()
        {
            return Task.Run(() => true);
        }

        public Task<List<ModalidadCampo>> GetAllModalidades()
        {
            var lstResultado = RuletaMasivianContext.ModalidadCampo.ToList();
            return Task.Run(() => lstResultado);
        }

        public Task<List<Campo>> GetAll()
        {
            var lstResultado = RuletaMasivianContext.Campo.OrderByDescending(x => x.RowCreatedDate).ToList();
            return Task.Run(() => lstResultado);
        }

        public Task<List<Campo>> GetAllByUserId(string user)
        {
            var lstResultado = RuletaMasivianContext.Campo.Where(l => l.RowCreatedBy == user || l.RowChangedBy == user).OrderByDescending(x => x.RowCreatedDate).ToList();
            return Task.Run(() => lstResultado);
        }

        public Task<List<LeyRegalias>> GetAllLeyRegalias()
        {
            var lstResultado = RuletaMasivianContext.LeyRegalias.ToList();
            return Task.Run(() => lstResultado);
        }

        public Task<List<Campo>> GetById(int id)
        {
            var lstResultado = new List<Campo>
            {
                RuletaMasivianContext.Campo.Find(id)
            };

            return Task.Run(() => lstResultado);
        }

        public async Task<bool> Update(Campo campoContrato)
        {
            var local = RuletaMasivianContext.Set<Campo>().Local.FirstOrDefault(entry => entry.Id.Equals(campoContrato.Id));
            if (local != null)
            {
                RuletaMasivianContext.Entry(local).State = EntityState.Detached;
            }
            RuletaMasivianContext.Entry(campoContrato).State = EntityState.Modified;
            string idBDP = RuletaMasivianContext.Campo.Where(x => x.Id == campoContrato.Id).Select(x => x.IdCampo).FirstOrDefault();
            if (idBDP != "0" && idBDP != null) { campoContrato.IdCampo = idBDP; campoContrato.Descripcion = "ACTUALIZACIÓN"; }
            RuletaMasivianContext.SaveChanges();
            string rutaAprobacion = RuletaMasivianContext.Parametros.Where(x => x.NombreParametro == "rutaAprobador").Select(x => x.ParametroVarchar).FirstOrDefault();
            if (campoContrato.NombreEstado.ToUpper() != "DEVUELTO" && campoContrato.NombreEstado.ToUpper() != "RECHAZADO")
            {
                await _sendMailService.SendEmailAsync(new EmailInfo()
                {
                    Body = string.Format("<h3>Se notifica el recurso 'Campo Contrato - {0}', en estado: {1}, <br/> si corresponde, <a href='{2}'>ingrese para su aprobación</h3></a>",
                 campoContrato.NombreCampo, campoContrato.NombreEstado, rutaAprobacion),
                    Subject = string.Format("Recurso {0} desde Integrador Volúmetrico BDP", campoContrato.Descripcion),
                    To = RuletaMasivianContext.Parametros.Where(x => x.NombreParametro == "correoAprobador").Select(x => x.ParametroVarchar).FirstOrDefault()
                });
            }
            return await Task.Run(() => true);
        }
    }
}
