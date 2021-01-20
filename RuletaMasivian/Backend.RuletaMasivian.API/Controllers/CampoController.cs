using Backend.RuletaMasivian.Entities.Interface.Business;
using Backend.RuletaMasivian.Entities.Models;
using Backend.RuletaMasivian.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.RuletaMasivian.API.Controllers
{
    /// <summary>
    /// CampoController
    /// </summary>
    /// <seealso cref="Backend.RuletaMasivian.API.Controllers.Base.BaseController" />
    public class CampoController : Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly ICampoBusiness _business;

        /// <summary>
        /// Initializes a new instance of the <see cref="CampoController"/> class.
        /// </summary>
        /// <param name="business">The business.</param>
        public CampoController(ICampoBusiness business)
        {
            _business = business;
        }

        /// <summary>
        /// Campo. Genera listado de todos los campos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var user = GetUserId;
            var rolesIds = GetRolesIds();
            var result = await _business.GetAll(user, rolesIds);

            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Campo. Permite buscar un campo por id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _business.GetById(id);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Campo. Permite crear un nuevo campo
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(Campo campo)
        {
            campo.CorreoElectronico = GetEmail;
            campo.NombreEstado = "En proceso de aprobación";
            SessionHelper.AddUserAndDate(ref campo, GetUserId);
            var result = await _business.Create(campo);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Campo. Permite modificar un campo
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update(Campo campo)
        {
            campo.CorreoElectronico = GetEmail;
            campo.NombreEstado = "En proceso de aprobación";
            SessionHelper.AddUserAndDateUpdate(ref campo, GetUserId);
            var result = await _business.Update(campo);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Campo. Aprobación de Campos
        /// </summary>
        /// <param name="campos">The campos.</param>
        /// <returns></returns>
        [Route("Aprobacion")]
        [HttpPost]
        public async Task<ActionResult> Aprobacion([FromBody] IEnumerable<Campo> campos)
        {
            string Email = GetEmail;
            List<Campo> lst = campos.ToList();
            var result = await _business.Aprobacion(lst, Email);
            return StatusCode(result.Code, result);
        }
    }
}