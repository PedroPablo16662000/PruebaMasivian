using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RuletaPruebaMasivian.Interface.IBusiness;
using RuletaPruebaMasivian.Models;

namespace RuletaPruebaMasivian.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuletaController : ControllerBase
    {
        public IRuletaBusiness _business;
        public IDistributedCache _distributeCache;

        private readonly ILogger<RuletaController> _logger;
        public RuletaController(IRuletaBusiness business, IDistributedCache distributeCache, ILogger<RuletaController> logger)
        {
            _distributeCache = distributeCache;
            _business = business;
            _logger = logger;
        }
        [HttpPost("Add")]
        public int Crea(Ruleta ruleta) {
            int idRuleta = _business.Add(ruleta);
            return idRuleta;
        }
        [HttpGet]
        public IEnumerable<Ruleta> Get()
        {
            var Ruleta = new List<Ruleta>();
            Ruleta.Add(new Ruleta()
            {
                idRuleta = 1,
                fechaInicial = DateTime.Now,
                estadoActual = true,
                marca = "Casino Royal",
                observacion = "Perfecto estado"
            });
            return Ruleta;
        }

        [HttpPut("AbrirRuleta")]
        public string Put(int ruleta)
        {
            return _business.OpenRuleta(ruleta);
        }
    }
}
