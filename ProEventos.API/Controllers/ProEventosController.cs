using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly ProEventosContext proEventosContext;

        public EventosController(ProEventosContext context)
        {
            proEventosContext = context;
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return proEventosContext.Eventos;
        }

        [HttpGet("{id}")]
        public Evento GetById(int id)
        {
            return proEventosContext.Eventos.FirstOrDefault(e => e.Id.Equals(id));
        }
    }
}
