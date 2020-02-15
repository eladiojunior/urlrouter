using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrlRouter.AspNetMvc.Models
{
    public class RotaUrlViewModel
    {
        [Display(Name = "Chave rota")]
        public string ChaveRota { get; set; }
        [Display(Name = "Nome rota")]
        public string NomeRota { get; set; }
        [Display(Name = "Descrição rota")]
        public string DescricaoRota { get; set; }
        [Display(Name = "Url destino")]
        public string UrlDestino { get; set; }
        [Display(Name = "Rotas para Sistemas Operacionais móveis?")]
        public bool HasRotasSoMobile { get; set; }
        [Display(Name = "Url destino iOS")]
        public string UrlDestinoIOS { get; set; }
        [Display(Name = "Url destino Android")]
        public string UrlDestinoAndroid { get; set; }
        [Display(Name = "Url destino Windows Phone")]
        public string UrlDestinoWindowsPhone { get; set; }
        [Display(Name = "Guardar informações de acesso a Url?")]
        public bool HasControleAcesso { get; set; }
        public bool HasRotaVigente { get; internal set; }
    }
}
