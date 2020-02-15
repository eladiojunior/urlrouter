using System;

namespace UrlRouter.WebApi.Models
{
    public class AcessoRotaUrlModel
    {
        public string Id { get; set; }
        public string ChaveRota { get; set; }
        public string IpOrigemAcesso { get; set; }
        public bool HasDispositivoMovel { get; set; }
        public string TipoDispositivoMovel { get; set; }
        public string ModeloDispositivoMovel { get; set; }
        public string SistemaOperacionalAcesso { get; set; }
        public string InformacoesOrigemAcesso { get; set; }
        public DateTimeOffset DataHoraAcesso { get; set; }
    }
}
