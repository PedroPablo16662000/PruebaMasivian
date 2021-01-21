using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApuestaPruebaMasivian.Interface.IBusiness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RuletaPruebaMasivian.Models;

namespace RuletaPruebaMasivian.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApuestaController : ControllerBase
    {
        private readonly ILogger<ApuestaController> _logger;
        private IApuestaBusiness _business;
        public ApuestaController(ILogger<ApuestaController> logger, IApuestaBusiness business)
        {
            _logger = logger;
            _business = business;
        }
        [HttpGet]
        public IEnumerable<Apuesta> Get()
        {
            var rng = new Random();
            return new List<Apuesta>().ToArray();
        }
        [HttpPost]
        public int Post(Apuesta apuesta)
        {
            //apuesta.idUsuario = string.IsNullOrEmpty(apuesta.idUsuario)? Request.Headers["UserCode"]: apuesta.idUsuario;
            int apuestaId = _business.Add(apuesta);
            return apuestaId;
        }
        [HttpPut]
        public int Put(IEnumerable<Apuesta> apuesta)
        {
            return 1;
        }
    }
}
