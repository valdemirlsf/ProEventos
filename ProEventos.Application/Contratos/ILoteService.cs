using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface ILoteService
    {
        Task<LoteDto[]> AddLotes(int EventoId, LoteDto[] model);

        Task<bool> RemoveLotes(int eventoId, int loteId);

        Task<LoteDto[]> GetLotesByIdEventoAsync(int idEvento);

        Task<LoteDto> GetLoteByIdAsync(int EventoId, int id);
       
    }
}
