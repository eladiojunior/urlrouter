using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UrlRouter.AspNetMvc.Service;

namespace UrlRouter.AspNetMvc.Controllers
{
    public class DefaultController: Controller
    {
        public readonly IViewRenderService _viewRenderService;
        public readonly IUrlRouterApiClient _urlRouterApiClient;
        public readonly IConfiguration _configuration;
        public DefaultController(IConfiguration configuration, IViewRenderService viewRenderService, IUrlRouterApiClient urlRouterApiClient)
        {
            this._viewRenderService = viewRenderService;
            this._urlRouterApiClient = urlRouterApiClient;
            this._configuration = configuration;
        }

        // <summary>
        /// Cria um retorno Json de Erro (Result = false) com mensagem de erro.
        /// </summary>
        /// <param name="mensagemErro">Mensagem de erro que deve ser apresentada ao usuário.</param>
        /// <returns></returns>
        internal JsonResult JsonResultErro(string mensagemErro)
        {
            return Json(new { HasErro = true, Erros = new List<string> { mensagemErro } });
        }

        /// <summary>
        ///     Cria um retorno Json de Erro (Result = false) com mensagem de erro.
        /// </summary>
        /// <param name="mensagensErro">Resultado de mensagens de erro que deve ser apresentada ao usuário.</param>
        /// <returns></returns>
        internal JsonResult JsonResultErro(IEnumerable mensagensErro)
        {
            return Json(new { HasErro = true, Erros = mensagensErro });
        }

        internal JsonResult JsonResultErro(object model, string mensagem = "")
        {
            return Json(new { HasErro = true, Model = model, Mensagem = mensagem });
        }

        internal JsonResult JsonResultErro(Exception ex)
        {
            return Json(new { HasErro = true, Erros = new[] { ex.Message } });
        }

        /// <summary>
        ///     Cria um retorno Json de Erro (Result = false) com mensagem de erro, com base nos erros do modelState
        /// </summary>
        /// <param name="mensagensErro">Resultado de mensagens de erro que deve ser apresentada ao usuário.</param>
        /// <returns></returns>
        internal JsonResult JsonResultErro(ModelStateDictionary modelState)
        {
            IQueryable<string> chaves = from modelstate in modelState.AsQueryable().Where(f => f.Value.Errors.Count > 0) select modelstate.Key;
            IQueryable<ModelError> mensagens =
                from modelstate in modelState.AsQueryable().Where(f => f.Value.Errors.Count > 0)
                select modelstate.Value.Errors.FirstOrDefault(a => !string.IsNullOrEmpty(a.ErrorMessage));
            return
                Json(
                    new
                    {
                        HasErro = true,
                        Chaves = chaves,
                        Erros = mensagens.Where(a => a != null).Select(a => a.ErrorMessage).ToList()
                    });
        }

        /// <summary>
        ///     Cria um retorno Json de Sucesso (Result = true) com mensagem para o usuário (opcional).
        /// </summary>
        /// <param name="mensagemAlerta">Mensagem de alerta que deve ser apresentada ao usuário.</param>
        /// <returns></returns>
        internal JsonResult JsonResultSucesso(string mensagemAlerta = "")
        {
            return Json(new { HasErro = false, Mensagem = mensagemAlerta });
        }

        /// <summary>
        ///     Cria um retorno Json de Sucesso (Result = true) com Model e mensagem para o usuário (opcional).
        /// </summary>
        /// <param name="model">Informações do Model para renderizar a view.</param>
        /// <param name="mensagemAlerta">Mensagem de alerta que deve ser apresentada ao usuário.</param>
        /// <returns></returns>
        internal JsonResult JsonResultSucesso(object model, string mensagemAlerta = "")
        {
            return Json(new { HasErro = false, Model = model, Mensagem = mensagemAlerta });
        }
    }

}