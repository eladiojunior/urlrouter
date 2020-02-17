using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrlRouter.WebApi.Contexto.Entites;
using UrlRouter.WebApi.Contexto.Repositores;
using UrlRouter.WebApi.Models;

namespace UrlRouter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcessoRotaUrlController : ControllerBase
    {
        private IAcessoRotaUrlRepository AcessoRotaUrlRepository { get; set; }

        public AcessoRotaUrlController(IAcessoRotaUrlRepository repository)
        {
            AcessoRotaUrlRepository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string chave)
        {
            IEnumerable<AcessoRotaUrlEntity> entites = await AcessoRotaUrlRepository.GetAll(chave);
            return Ok(ConvertModels.ConverterToListModel(entites));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var item = await AcessoRotaUrlRepository.Get(id);
            if (item == null)
                return NotFound($"Id de Acesso a Rota Url [{id}] não encontrada.");
            return Ok(ConvertModels.ConverterToModel(item));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcessoRotaUrlModel model)
        {
            if (model == null)
                return BadRequest("Informações de Acesso a Rota Url não preenchidas.");
            if (string.IsNullOrEmpty(model.ChaveRota))
                return BadRequest("Chave da rota url não informada, obrigatório.");
            if (model.DataHoraAcesso == null)
                model.DataHoraAcesso = DateTime.Now;
            var entity = ConvertModels.ConverterToEntity(model);
            await AcessoRotaUrlRepository.Create(entity);
            return Ok($"Acesso a rota Url [{entity.Id}] registrado com sucesso.");
        }

    }
}