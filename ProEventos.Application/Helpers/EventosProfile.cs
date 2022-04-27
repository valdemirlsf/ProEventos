using AutoMapper;
using ProEventos.Domain;
using ProEventos.Application.Dtos;
namespace ProEventos.Application.Helpers
{
    public class EventosProfile : Profile
    {
        public EventosProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Palestrante, PalestranteDto>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<Lote, LoteDto>().ReverseMap();
        }
    }
}
