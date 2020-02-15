using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlRouter.AspNetMvc.Service.Models;

namespace UrlRouter.AspNetMvc.Models
{
    internal class ConvertModels
    {
        internal static RotaUrlViewModel ConvertToModel(RotaUrlModelApi modelApi)
        {
            var model = new RotaUrlViewModel();
            model.ChaveRota = modelApi.Chave;
            model.NomeRota = modelApi.Nome;
            model.DescricaoRota = modelApi.Descricao;
            model.UrlDestino = modelApi.UrlDestino;
            model.UrlDestinoIOS = modelApi.UrlDestinoIOS;
            model.UrlDestinoAndroid = modelApi.UrlDestinoAndroid;
            model.UrlDestinoWindowsPhone = modelApi.UrlDestinoWindowsPhone;
            model.HasControleAcesso = modelApi.HasControleAcesso;
            model.HasRotaVigente = modelApi.HasRotaVigente;
            return model;
        }
    }
}
