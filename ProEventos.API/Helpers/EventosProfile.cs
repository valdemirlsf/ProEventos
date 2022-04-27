using AutoMapper;
using ProEventos.API.Dtos;
using ProEventos.Domain;
namespace ProEventos.API.Helpers
{
    public class EventosProfile : Profile
    {
        public EventosProfile()
        {
            CreateMap<Evento, EventoDto>();
        }
    }
}
