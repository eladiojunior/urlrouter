namespace UrlRouter.AspNetMvc.Service.Models
{
    public class RotaUrlModelApi
    {
        public string Id { get; set; }
        public string Chave { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string UrlDestino { get; set; }
        public string UrlDestinoIOS { get; set; }
        public string UrlDestinoAndroid { get; set; }
        public string UrlDestinoWindowsPhone { get; set; }
        public bool HasControleAcesso { get; set; }
        public bool HasRotaVigente { get; set; }
    }
}
