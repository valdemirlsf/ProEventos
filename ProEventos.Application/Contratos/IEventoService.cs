using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(EventoDto model);
        Task<bool> RemoveEventos(int eventoId);
        Task<EventoDto> UpdateEvento(int eventoId, EventoDto model);

        Task<EventoDto[]> GetAllEventsAsync(bool includePalestrantes = true);

        Task<EventoDto> GetEventByIdAsync(int EventoId, bool includePalestrantes = true);
        Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = true);

    }
}
