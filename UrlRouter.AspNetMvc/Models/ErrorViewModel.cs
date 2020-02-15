using System;

namespace UrlRouter.AspNetMvc.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string Descricao { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}