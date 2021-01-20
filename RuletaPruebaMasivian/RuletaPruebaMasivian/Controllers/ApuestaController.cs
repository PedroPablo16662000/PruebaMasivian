using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public ApuestaController(ILogger<ApuestaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Apuesta> Get()
        {
            var rng = new Random();
            return new List<Apuesta>().ToArray();
        }
        [HttpPost]
        public int Post(IEnumerable<Apuesta> apuesta) {
            return 1;
        }
        [HttpPut]
        public int Put(IEnumerable<Apuesta> apuesta)
        {
            return 1;
        }
    }
}
