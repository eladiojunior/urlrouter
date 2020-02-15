using System;
using System.Collections.Generic;
using System.Text;
using UrlRouter.Core.Dados.Model;

namespace UrlRouter.Core.Negocio.DTOs
{
    public class HistoricoAcessoDTO
    {
        public string ChaveRota { get; set; }
        public string IpOrigemAcesso { get; set; }
        public bool HasDispositivoMovel { get; set; }
        public string TipoDispositivoMovel { get; set; }
        public string ModeloDispositivoMovel { get; set; }
        public string SistemaOperacionalAcesso { get; set; }
        public string InformacoesOrigemAcesso { get; set; }
        public DateTime DataHoraAcesso { get; set; }

        public static HistoricoAcessoDTO ConvertToDto(HistoricoAcessoRotaUrlModel model)
        {
            HistoricoAcessoDTO dto = null;
            if (model != null)
            {
                DateTime dataCorrente = DateTime.Now;
                dto = new HistoricoAcessoDTO();
                dto.ChaveRota = model.ChaveRota;
                dto.IpOrigemAcesso = model.IpOrigemAcesso;
                dto.HasDispositivoMovel = model.HasDispositivoMovel;
                dto.TipoDispositivoMovel = model.TipoDispositivoMovel;
                dto.ModeloDispositivoMovel = model.ModeloDispositivoMovel;
                dto.SistemaOperacionalAcesso = model.SistemaOperacionalAcesso;
                dto.InformacoesOrigemAcesso = model.InformacoesOrigemAcesso;
                dto.DataHoraAcesso = model.DataHoraAcesso;
            }
            return dto;
        }

        public static List<HistoricoAcessoDTO> ConvertToDto(List<HistoricoAcessoRotaUrlModel> models)
        {
            List<HistoricoAcessoDTO> listDtos = null;
            if (models == null)
                return listDtos;
            listDtos = new List<HistoricoAcessoDTO>();
            foreach (var item in models)
            {
                listDtos.Add(ConvertToDto(item));
            }
            return listDtos;
        }
    }
}
