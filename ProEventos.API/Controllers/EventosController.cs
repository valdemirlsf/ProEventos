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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        private readonly IWebHostEnvironment _env;

        private readonly string _destino = "Images";

        public EventosController(IEventoService eventoService, IWebHostEnvironment env)
        {
            _env = env;
           _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try {
                var eventos = await _eventoService.GetAllEventsAsync();
                if(eventos == null)
                {
                    return NotFound("Nenhum Evento Encontrado");
                }
                var eventoRetorno = new List<EventoDto>();
                return Ok(eventos);
            } catch(Exception ex) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
            
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventByIdAsync(id,true);
                if (evento == null)
                {
                    return NotFound("Evento não encontrado");
                }
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. Erro: {ex.Message}");
            }
        }

        [HttpGet("tema/{tema}")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosByTemaAsync(tema, true);
                if (eventos == null)
                {
                    return NotFound("Nenhum Evento Encontrado com este tema");
                }
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }


        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var eventos = await _eventoService.AddEventos(model);
                if (eventos == null)
                {
                    return BadRequest("Erro ao tentar adicionar evento");
                }
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar evento. Erro: {ex.Message}");
            }
        }

        [HttpPost("uploadfoto/{eventoId}")]
        public async Task<IActionResult> Post(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventByIdAsync(eventoId);
                if (evento == null)
                {
                    return BadRequest("Erro ao tentar adicionar evento");
                }
                var file = Request.Form.Files[0];
                if (file.Length>0) {
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await saveImage(file); 
                }

                var retorno = await _eventoService.UpdateEvento(eventoId, evento);
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar imagem. Erro: {ex.Message}");
            }
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit(int id, EventoDto model)
        {
            try
            {
                var eventos = await _eventoService.UpdateEvento(id, model);
                if (eventos == null)
                {
                    return BadRequest("Erro ao tentar adicionar evento");
                }
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar Atualizar evento. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _eventoService.RemoveEventos(id)){
                    return Ok("Evento Deletado");
                }
                else
                {
                    return BadRequest("Não foi possível deletar o evento");
                }
                

                
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar evento. Erro: {ex.Message}");
            }
        }

        [NonAction]
        public void DeleteImage(string imagemName)
        {
            var pathIm = Path.Combine(_env.ContentRootPath, @"Resources/images", imagemName);
            if (System.IO.File.Exists(pathIm))
            {
                System.IO.File.Delete(pathIm);
            }
        }

        [NonAction]
        public async Task<string> saveImage(IFormFile imagem)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imagem.FileName)
                .Take(10)
                .ToArray()
                ).Replace(" ", "_");

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssff")}{Path.GetExtension(imagem.FileName)}";
            var ImagePath = Path.Combine(_env.ContentRootPath, @"Resources/Image", imageName);

            using (var fs = new FileStream(ImagePath, FileMode.Create))
            {
                await imagem.CopyToAsync(fs);
            }
            return imageName;
        }
    }
}
