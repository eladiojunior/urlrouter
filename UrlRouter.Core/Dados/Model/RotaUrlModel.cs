using System;
using System.Collections.Generic;
using System.Text;

namespace UrlRouter.Core.Dados.Model
{
    [Serializable]
    public class RotaUrlModel : VirtualRepositorio.GenericVirtualEntity
    {
        public string Chave { get; set; }
        public string NomeRota { get; set; }
        public string Descricao { get; set; }
        public string UrlDestino { get; set; }
        public string UrlDestinoIOS { get; set; }
        public string UrlDestinoAndroid { get; set; }
        public string UrlDestinoWindowsPhone { get; set; }
        public bool HasControleAcesso { get; set; }
        public DateTime DataInicialVigencia { get; set; }
        public DateTime? DataFinalVigencia { get; set; }

    }
}
