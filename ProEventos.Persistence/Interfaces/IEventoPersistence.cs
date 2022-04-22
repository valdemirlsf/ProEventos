using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Interfaces
{
    internal interface IEventoPersistence
    {
        //Eventos
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);

        Task<Evento[]> GetAllEventsAsync(string tema, bool includePalestrantes);

        Task<Evento> GetEventByIdAsync(int EventoId, bool includePalestrantes);
    }
}
