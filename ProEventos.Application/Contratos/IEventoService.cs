using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<Evento> AddEventos(Evento model);
        Task<bool> RemoveEventos(int eventoId);
        Task<Evento> UpdateEvento(int eventoId, Evento model);

        Task<Evento[]> GetAllEventsAsync(bool includePalestrantes = true);

        Task<Evento> GetEventByIdAsync(int EventoId, bool includePalestrantes = true);
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = true);

    }
}
