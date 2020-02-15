using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrlRouter.AspNetMvc.Models;
using UrlRouter.AspNetMvc.Models.Filtros;
using UrlRouter.AspNetMvc.Service;

namespace UrlRouter.AspNetMvc.Controllers
{
    public class RotaUrlController : DefaultController
    {
        public RotaUrlController(IConfiguration configuration, IViewRenderService viewRenderService, IUrlRouterApiClient urlRouterApiClient) 
            : base (configuration, viewRenderService, urlRouterApiClient)
        {
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        public async Task<IActionResult> Listar(FiltroRotaUrlModel model)
        {
            List<RotaUrlViewModel> viewModel = new List<RotaUrlViewModel>();
            try
            {
                var listResultApi = _urlRouterApiClient.Listar(model.Nome, model.HasVigentes);
                if (listResultApi != null)
                {
                    foreach (var item in listResultApi)
                    {
                        viewModel.Add(ConvertModels.ConvertToModel(item));
                    }
                }
                var result = await _viewRenderService.RenderToStringAsync("RotaUrl/_Lista", viewModel);
                return JsonResultSucesso(result, string.Empty);
            }
            catch (Exception erro)
            {
                return JsonResultErro(erro);
            }
        }
    }
}
