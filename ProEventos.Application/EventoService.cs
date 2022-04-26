using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersistence _geralPersistence;
        private readonly IEventoPersistence _eventoPersistence;

        public EventoService(IGeralPersistence geralPersistence, IEventoPersistence eventoPersistence)
        {
            _geralPersistence = geralPersistence;
            _eventoPersistence = eventoPersistence;
        }

        public async Task<Evento> AddEventos(Evento model)
        {
            try {
                _geralPersistence.Add<Evento>(model);
                if(await _geralPersistence.SaveChangesAsync())
                {
                    return await _eventoPersistence.GetEventByIdAsync(model.Id, true);
                }
                return null;
            }catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = true)
        {
            try
            {
                var eventos = await _eventoPersistence.GetAllEventosByTemaAsync(tema,includePalestrantes);
                if (eventos == null) { return null; }
                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventsAsync(bool includePalestrantes = true)
        {
            try {
                var eventos = await _eventoPersistence.GetAllEventsAsync(includePalestrantes);
                if (eventos == null) { return null; }
                return eventos;
            }catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetEventByIdAsync(int eventoId, bool includePalestrantes = true)
        {
            try
            {
                var eventos = await _eventoPersistence.GetEventByIdAsync(eventoId,includePalestrantes);
                if (eventos == null) { return null; }
                return eventos;
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

        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventoPersistence.GetEventByIdAsync(eventoId, false);
                if (evento == null) { return null; }

                model.Id = evento.Id;

                _geralPersistence.Update(model);
                if (await _geralPersistence.SaveChangesAsync()) {
                    return await _eventoPersistence.GetEventByIdAsync(model.Id, false); 
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
