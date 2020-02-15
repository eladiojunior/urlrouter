using System;
using System.Collections.Generic;
using UrlRouter.Core.Dados.Model;

namespace UrlRouter.Core.Negocio.DTOs
{
    public class UrlDTO
    {
        public string Chave { get; set; }
        public string NomeRota { get; set; }
        public string Descricao { get; set; }
        public string UrlDestino { get; set; }
        public string UrlDestinoIOS { get; set; }
        public string UrlDestinoAndroid { get; set; }
        public string UrlDestinoWindowsPhone { get; set; }
        public bool HasControleAcesso { get; set; }
        public bool HasRotaVigente { get; set; }

        public static UrlDTO ConvertToDto(RotaUrlModel model)
        {
            UrlDTO dto = null;
            if (model != null)
            {
                DateTime dataCorrente = DateTime.Now;
                dto = new UrlDTO();
                dto.Chave = model.Chave;
                dto.NomeRota = model.NomeRota;
                dto.Descricao = model.Descricao;
                dto.UrlDestino = model.UrlDestino;
                dto.UrlDestinoIOS = model.UrlDestinoIOS;
                dto.UrlDestinoAndroid = model.UrlDestinoAndroid;
                dto.UrlDestinoWindowsPhone = model.UrlDestinoWindowsPhone;
                dto.HasControleAcesso = model.HasControleAcesso;
                dto.HasRotaVigente = (model.DataInicialVigencia >= dataCorrente && (model.DataFinalVigencia != null || model.DataFinalVigencia <= dataCorrente));
            }
            return dto;
        }

        public static List<UrlDTO> ConvertToDto(List<RotaUrlModel> models)
        {
            List<UrlDTO> listDtos = null;
            if (models == null)
                return listDtos;
            listDtos = new List<UrlDTO>();
            foreach (var item in models)
            {
                listDtos.Add(ConvertToDto(item));
            }
            return listDtos;
        }
    }
}
