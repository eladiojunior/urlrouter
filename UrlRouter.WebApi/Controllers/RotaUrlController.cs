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
    public class RotaUrlController : ControllerBase
    {
        private IRotaUrlRepository RotaUrlRepository { get; set; }

        public RotaUrlController(IRotaUrlRepository repository)
        {
            RotaUrlRepository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string nome, bool hasVigentes)
        {
            IEnumerable<RotaUrlEntity> rotasEntity = await RotaUrlRepository.GetAll(nome, hasVigentes);
            return Ok(ConvertModels.ConverterToListModel(rotasEntity));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await RotaUrlRepository.GetById(id);
            if (item == null)
                return NotFound($"Id da rota URL [{id}] não encontrada.");
            return Ok(ConvertModels.ConverterToModel(item));
        }

        [HttpGet("{chave}")]
        public async Task<IActionResult> GetByChave(string chave)
        {
            var item = await RotaUrlRepository.GetByChave(chave);
            if (item == null)
                return NotFound($"Chave da rota URL [{chave}] não encontrada.");
            return Ok(ConvertModels.ConverterToModel(item));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RotaUrlModel model)
        {
            if (model == null)
                return BadRequest("Informações da rota URL não preenchidas.");
            if (string.IsNullOrEmpty(model.Nome))
                return BadRequest("Nome da rota URL não informada, obrigatório.");
            var entity = ConvertModels.ConverterToEntity(model);
            entity.Chave = ObterNovaChaveRota();
            await RotaUrlRepository.Create(entity);
            return Ok($"Rota URL [chave: {entity.Chave}] registrada com sucesso.");
        }

        /// <summary>
        /// Criar uma chave de acesso a rota.
        /// </summary>
        /// <returns></returns>
        private string ObterNovaChaveRota()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RotaUrlModel model)
        {
            if (model == null)
                return BadRequest("Informações da rota URL não preechidas.");
            if (string.IsNullOrEmpty(model.Id))
                return BadRequest("Identificador da rota URL não informado, obrigatório.");
            var entity = await RotaUrlRepository.GetById(model.Id);
            if (entity == null)
                return NotFound($"Id da rota URL [{model.Id}] não encontrada, impossível atualizar.");
            //Atualizar os campos enviados na model.
            entity.Nome = model.Nome;
            entity.Descricao = model.Descricao;
            entity.UrlDestino = model.UrlDestino;
            entity.UrlDestinoIOS = model.UrlDestinoIOS;
            entity.UrlDestinoAndroid = model.UrlDestinoAndroid;
            entity.UrlDestinoWindowsPhone = model.UrlDestinoWindowsPhone;
            entity.HasControleAcesso = model.HasControleAcesso;
            entity.DataInicialVigencia = model.DataInicialVigencia;
            entity.DataFinalVigencia = model.DataFinalVigencia;
            //Atualizar
            bool hasResult = await RotaUrlRepository.Update(entity);
            if (!hasResult)
                return NotFound("Não foi possível atualizar a rota URL.");
            return Ok($"Rota URL [chave: {model.Chave}] atualizada com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool hasResult = await RotaUrlRepository.Delete(id);
            if (!hasResult)
                return NotFound("Não foi possível remover a rota URL.");
            return Ok($"Rota URL [id: {id}] removida com sucesso.");
        }
    }
}