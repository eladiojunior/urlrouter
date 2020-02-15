using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UrlRouter.Core.Dados.Model;

namespace UrlRouter.Core.Dados.Repositorio
{
    internal class Repositorios
    {
        private VirtualRepositorio.Repositorio<RotaUrlModel> repositorioRotas = null;
        private VirtualRepositorio.Repositorio<HistoricoAcessoRotaUrlModel> repositorioHistorico = null;
        public Repositorios(IConfiguration iConfig)
        {
            repositorioRotas = VirtualRepositorio.VirtualRepositorio<RotaUrlModel>.Get(iConfig).GetRepositorio();
            repositorioHistorico = VirtualRepositorio.VirtualRepositorio<HistoricoAcessoRotaUrlModel>.Get(iConfig).GetRepositorio();
        }
        public VirtualRepositorio.Repositorio<RotaUrlModel> RotasUrl()
        {
            return repositorioRotas;
        }
        public VirtualRepositorio.Repositorio<HistoricoAcessoRotaUrlModel> HistoricoAcessos()
        {
            return repositorioHistorico;
        }
    }
}
