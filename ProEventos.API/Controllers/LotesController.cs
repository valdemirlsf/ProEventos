using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _loteService;

        public LotesController(ILoteService lotesService)
        {

            _loteService = lotesService;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var eventos = await _loteService.GetLotesByIdEventoAsync(eventoId);
                if (eventos == null)
                {
                    return NotFound("Nenhum Evento Encontrado");
                }
                var eventoRetorno = new List<EventoDto>();
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar lotes. Erro: {ex.Message}");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, LoteDto[] models)
        {
            try
            {
                var lotes = await _loteService.AddLotes(id, models);
                if (lotes == null)
                {
                    return BadRequest("Erro ao tentar adicionar lotes");
                }
                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteService.GetLoteByIdAsync(eventoId, loteId);
                if(lote == null) { return NoContent(); }

                return await _loteService.RemoveLotes(lote.EventoId, lote.Id)
                       ? Ok(new { message = "Lote Deletado" })
                       : throw new Exception("Ocorreu um problem não específico ao tentar deletar Lote.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar lotes. Erro: {ex.Message}");
            }
        }

    }
}
