using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Interfaces;
using System;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersistence _geralPersistence;
        private readonly IEventoPersistence _eventoPersistence;
        private readonly IMapper _mapper;

        public EventoService(IGeralPersistence geralPersistence, IEventoPersistence eventoPersistence, IMapper mapper)
        {
            _mapper = mapper;
            _geralPersistence = geralPersistence;
            _eventoPersistence = eventoPersistence;
        }

        public async Task<EventoDto> AddEventos(EventoDto model)
        {
            try {

                var evento = _mapper.Map<Evento>(model);
                _geralPersistence.Add<Evento>(evento);
                if(await _geralPersistence.SaveChangesAsync())
                {
                    var retorno =  await _eventoPersistence.GetEventByIdAsync(evento.Id, true);
                    return _mapper.Map<EventoDto>(retorno);
                }
                return null;
            }catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = true)
        {
            try
            {
                var eventos = await _eventoPersistence.GetAllEventosByTemaAsync(tema,includePalestrantes);
                if (eventos == null) { return null; }
                var eventoMapped = _mapper.Map<EventoDto[]>(eventos);

                return eventoMapped;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventsAsync(bool includePalestrantes = true)
        {
            try {
                var eventos = await _eventoPersistence.GetAllEventsAsync(includePalestrantes);
                if (eventos == null) { return null; }

                var eventoMapped = _mapper.Map<EventoDto[]>(eventos);

                return eventoMapped;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventByIdAsync(int eventoId, bool includePalestrantes = true)
        {
            try
            {
                var eventos = await _eventoPersistence.GetEventByIdAsync(eventoId,includePalestrantes);
                if (eventos == null) { return null; }
                var eventoMapped = _mapper.Map<EventoDto>(eventos);

                return eventoMapped;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveEventos(int eventoId)
        {
            try
            {
                var evento = await _eventoPersistence.GetEventByIdAsync(eventoId, false);
                if (evento == null)  throw new Exception("O objeto não foi encontrado para deleção"); 


                _geralPersistence.Delete<Evento>(evento);
                return await _geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersistence.GetEventByIdAsync(eventoId, false);
                if (evento == null) { return null; }

                model.Id = evento.Id;

                _mapper.Map(model, evento);

                _geralPersistence.Update(evento);
                if (await _geralPersistence.SaveChangesAsync()) {
                    var retorno = await _eventoPersistence.GetEventByIdAsync(evento.Id, true);
                    return _mapper.Map<EventoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
