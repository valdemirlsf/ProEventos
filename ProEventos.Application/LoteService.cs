using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Interfaces;
using AutoMapper;
using ProEventos.Domain;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly IGeralPersistence _geralPersistence;
        private readonly ILotePersistence _lotePersistence;
        IMapper _mapper;

        public LoteService(IGeralPersistence geralPersistence, ILotePersistence lotePersistence, IMapper mapper)
        {
            _geralPersistence = geralPersistence;
            _lotePersistence = lotePersistence;
            _mapper = mapper;

        }
        public async Task<LoteDto[]> AddLotes(int EventoId, LoteDto[] model)
        {
            try
            {
                var lotes = await _lotePersistence.GetLotesByEventoIdAsync(EventoId);
                if (lotes == null) { return null; }

                foreach(var lot in model)
                {
                    if (lot.Id == 0)
                    {
                        await AddLote(EventoId, lot);
                    }
                    else
                    {
                        var lote = lotes.FirstOrDefault(lote => lote.Id == lot.Id);
                        lot.EventoId = EventoId;
                        _mapper.Map(lot, lote);
                        _geralPersistence.Update<Lote>(lote);

                        await _geralPersistence.SaveChangesAsync();
                    }
                }
                var loteRetorno = await _lotePersistence.GetLotesByEventoIdAsync(EventoId);
                return _mapper.Map<LoteDto[]>(loteRetorno);
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto> AddLote(int EventoId, LoteDto model)
        {
            try
            {

                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = EventoId;
                _geralPersistence.Add<Lote>(lote);
                await _geralPersistence.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto> GetLoteByIdAsync(int EventoId, int id)
        {
            try
            {
                var lote = await _lotePersistence.GetLoteByIdAsync(EventoId, id);
                if (lote == null) { return null; }
                var result = _mapper.Map<LoteDto>(lote);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> GetLotesByIdEventoAsync(int idEvento)
        {
            try
            {
                var lotes = await _lotePersistence.GetLotesByEventoIdAsync(idEvento);
                if (lotes == null) { return null; }

                var result = _mapper.Map<LoteDto[]>(lotes);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveLotes(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersistence.GetLoteByIdAsync(eventoId, loteId);
                if (lote == null) throw new Exception("O objeto não foi encontrado para deleção");


                _geralPersistence.Delete<Lote>(lote);
                return await _geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
