using System;

namespace UrlRouter.Core.Dados.Model
{
    [Serializable]
    public class HistoricoAcessoRotaUrlModel : VirtualRepositorio.GenericVirtualEntity
    {
        public string ChaveRota { get; set; }
        public string IpOrigemAcesso { get; set; }
        public bool HasDispositivoMovel { get; set; }
        public string TipoDispositivoMovel { get; set; }
        public string ModeloDispositivoMovel { get; set; }
        public string SistemaOperacionalAcesso { get; set; }
        public string InformacoesOrigemAcesso { get; set; }
        public DateTime DataHoraAcesso { get; set; }
    }
}
