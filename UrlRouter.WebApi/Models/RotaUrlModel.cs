using System;

namespace UrlRouter.WebApi.Models
{
    public class RotaUrlModel
    {
        public string Id { get; set; }
        public string Chave { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string UrlDestino { get; set; }
        public string UrlDestinoIOS { get; set; }
        public string UrlDestinoWindowsPhone { get; set; }
        public string UrlDestinoAndroid { get; set; }
        public bool HasControleAcesso { get; set; }
        public DateTimeOffset DataInicialVigencia { get; set; }
        public DateTimeOffset? DataFinalVigencia { get; set; }
    }
}
