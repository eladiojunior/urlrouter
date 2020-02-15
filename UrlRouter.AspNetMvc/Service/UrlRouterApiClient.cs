using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlRouter.AspNetMvc.Service.Models;

namespace UrlRouter.AspNetMvc.Service
{

    public interface IUrlRouterApiClient
    {
        ICollection<RotaUrlModelApi> Listar(string nome, bool hasVigentes);
        void Gravar(RotaUrlModelApi model);
        RotaUrlModelApi Obter(string chave);
        void Remover(string idRota);
        void RegistrarAcesso(AcessoRotaUrlModelApi model);
    }

    public class UrlRouterApiClient : IUrlRouterApiClient
    {
        public void Gravar(RotaUrlModelApi model)
        {
            throw new NotImplementedException();
        }

        public ICollection<RotaUrlModelApi> Listar(string nome, bool hasVigentes)
        {
            throw new NotImplementedException();
        }

        public RotaUrlModelApi Obter(string idRota)
        {
            throw new NotImplementedException();
        }

        public void RegistrarAcesso(AcessoRotaUrlModelApi model)
        {
            throw new NotImplementedException();
        }

        public void Remover(string idRota)
        {
            throw new NotImplementedException();
        }
    }
}
