using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using UrlRouter.AspNetMvc.Models;
using UrlRouter.AspNetMvc.Service;
using UrlRouter.AspNetMvc.Service.Models;

namespace UrlRouter.AspNetMvc.Controllers
{
    public class HomeController : DefaultController
    {
        public HomeController(IConfiguration configuration, IViewRenderService viewRenderService, IUrlRouterApiClient urlRouterApiClient)
            : base(configuration, viewRenderService, urlRouterApiClient)
        {
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet]
        [Route("/Rota/{chave}")]
        public IActionResult Rota(string chave)
        {
            
            if (string.IsNullOrEmpty(chave))
                return View("Error", new ErrorViewModel { Descricao = "Nenhuma chave de rota foi informada para verificação." });
            
            RotaUrlModelApi rotaUrlModel = _urlRouterApiClient.Obter(chave);
            if (rotaUrlModel == null)
                return View("Error", new ErrorViewModel { Descricao = $"Chave da rota [{chave}] não encontrada, infelizmente não conseguimos realizar o roteamento." });
            if (!rotaUrlModel.HasRotaVigente)
                return View("Error", new ErrorViewModel { Descricao = $"Url com chave da rota [{chave}] não encontrada-se vigente, infelizmente não conseguimos realizar o roteamento." });

            string userAgent = Request.Headers["User-Agent"].ToString();
            bool hasDispositivoMovel = Helper.UtilHelper.HasDeviceMobile(userAgent);
            string sistemaOperacional = Helper.UtilHelper.ObterSistemaOperacional(userAgent);
            
            string urlDestino = rotaUrlModel.UrlDestino;

            if (hasDispositivoMovel && 
                (!string.IsNullOrEmpty(rotaUrlModel.UrlDestinoIOS) ||
                !string.IsNullOrEmpty(rotaUrlModel.UrlDestinoAndroid) ||
                !string.IsNullOrEmpty(rotaUrlModel.UrlDestinoWindowsPhone)))
            {//Direcionar para urls, conforme o tipo de sistema operacional.
                if (!string.IsNullOrEmpty(rotaUrlModel.UrlDestinoIOS) && sistemaOperacional.ToUpper().Contains("IOS"))
                    urlDestino = rotaUrlModel.UrlDestinoIOS;
                if (!string.IsNullOrEmpty(rotaUrlModel.UrlDestinoAndroid) && sistemaOperacional.ToUpper().Contains("ANDROID"))
                    urlDestino = rotaUrlModel.UrlDestinoAndroid;
                if (!string.IsNullOrEmpty(rotaUrlModel.UrlDestinoWindowsPhone) && sistemaOperacional.ToUpper().Contains("WINDOWS PHONE"))
                    urlDestino = rotaUrlModel.UrlDestinoWindowsPhone;
            }

            if (rotaUrlModel.HasControleAcesso)
            {//Registrar acesso a Url...

                AcessoRotaUrlModelApi acessoRotaModel = new AcessoRotaUrlModelApi();
                acessoRotaModel.ChaveRota = chave;
                acessoRotaModel.DataHoraAcesso = DateTime.Now;
                acessoRotaModel.InformacoesOrigemAcesso = userAgent;
                acessoRotaModel.HasDispositivoMovel = hasDispositivoMovel;
                if (hasDispositivoMovel)
                {
                    Helper.DeviceMobile deviceMobile = Helper.UtilHelper.ObterDeviceMobile(userAgent);
                    acessoRotaModel.TipoDispositivoMovel = deviceMobile.Device;
                    acessoRotaModel.ModeloDispositivoMovel = deviceMobile.Model;
                }
                acessoRotaModel.IpOrigemAcesso = Helper.UtilHelper.ObterIpMaquinaCliente(Request);
                acessoRotaModel.SistemaOperacionalAcesso = sistemaOperacional;
                _urlRouterApiClient.RegistrarAcesso(acessoRotaModel);
            }

            return Redirect(urlDestino);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
