using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence
{
    public class PalestrantePersistence : IPalestrantePersistence
    {
        public readonly ProEventosContext _context;

        public PalestrantePersistence(ProEventosContext context) {
            _context = context;
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = true)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.AsNoTracking().Include(e => e.RedesSociais);
            if (includeEventos)
            {
                query = query.Include(e => e.PalestrantesEventos);
            }
            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = true)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.AsNoTracking().Include(e => e.RedesSociais);
            if (includeEventos)
            {
                query = query.Include(e => e.PalestrantesEventos);
            }
            query = query.Where(e => e.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos = true)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.AsNoTracking().Include(e => e.RedesSociais);
            if (includeEventos)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }
            query = query.Where(e => e.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
