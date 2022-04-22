using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence
{
    internal class ProEventosPersistence : IPalestrantePersistence, IEventoPersistence, IGeralPersistence
    {
        private readonly ProEventosContext _context;

        public ProEventosPersistence(ProEventosContext proEventosContext)
        {
            _context = proEventosContext;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos=true)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedesSociais);
            if (includeEventos)
            {
                query = query.Include(e => e.PalestrantesEventos);
            }
            query = query.Where(e => e.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes=true)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.Lotes).Include(e => e.RedesSociais);
            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos);
            }
            query = query.Where(e => e.Tema.ToLower().Contains(tema.ToLower()));
            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventsAsync(string tema, bool includePalestrantes=true)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.Lotes).Include(e => e.RedesSociais);
            if (includePalestrantes) { 
                query = query.Include(e => e.PalestrantesEventos); 
            }
            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos=true)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedesSociais);
            if (includeEventos)
            {
                query = query.Include(e => e.PalestrantesEventos);
            }
            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventByIdAsync(int EventoId, bool includePalestrantes=true)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.Lotes).Include(e => e.RedesSociais);
            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestrantesEventos);
            }
            query = query.Where(e => e.Id == EventoId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos=true)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedesSociais);
            if (includeEventos)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }
            query = query.Where(e => e.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync())>0;
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
    }
}
