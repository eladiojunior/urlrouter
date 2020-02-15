using System;

namespace UrlRouter.Core.Negocio.Erro
{
    public class NegocioException : Exception
    {
        public NegocioException(string mensagem) : base(mensagem) {}
        public NegocioException(string mensagem, Exception innerException) : base(mensagem, innerException) {}
    }
}
