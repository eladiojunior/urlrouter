using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UrlRouter.Core.Dados.Model;
using UrlRouter.Core.Dados.Repositorio;
using UrlRouter.Core.Negocio.DTOs;
using UrlRouter.Core.Negocio.Erro;

namespace UrlRouter.Core.Negocio
{
    public class UrlRouterNegocio
    {
        private Repositorios repositorios;
        
        public UrlRouterNegocio(IConfiguration iConfig)
        {
            repositorios = new Repositorios(iConfig);
        }

        /// <summary>
        /// Registra um roteamento para uma URL informando confome as informações no objeto.
        /// </summary>
        /// <param name="url">Informações do roteamento da Url (obrigatório).</param>
        /// <returns>Chave (key) do roteamento registrado para utilização.</returns>
        public string RegistrarRotaUrl(UrlDTO url)
        {

            if (url == null)
                throw new NegocioException("Informações da Url não definidas.");
            if (string.IsNullOrEmpty(url.NomeRota))
                throw new NegocioException("Nome da rota não informado.");
            if (string.IsNullOrEmpty(url.UrlDestino))
                throw new NegocioException("Url de Destino da rota não informado.");

            RotaUrlModel rotaUrl = new RotaUrlModel();
            rotaUrl.Chave = ObterNovaChaveRota();
            rotaUrl.NomeRota = url.NomeRota;
            rotaUrl.Descricao = url.Descricao;
            rotaUrl.UrlDestino = url.UrlDestino;
            rotaUrl.UrlDestinoIOS = url.UrlDestinoIOS;
            rotaUrl.UrlDestinoAndroid = url.UrlDestinoAndroid;
            rotaUrl.HasControleAcesso = url.HasControleAcesso;
            rotaUrl.DataInicialVigencia = DateTime.Now.Date;
            rotaUrl.DataFinalVigencia = null;

            repositorios.RotasUrl().Incluir(rotaUrl);

            return rotaUrl.Chave;

        }

        /// <summary>
        /// Criar uma chave de acesso a rota.
        /// </summary>
        /// <returns></returns>
        private string ObterNovaChaveRota()
        {
            return Guid.NewGuid().ToString().Substring(0,8).ToUpper();
        }

        /// <summary>
        /// Obtem o roteamento de uma Url pela chave (key) informada.
        /// </summary>
        /// <param name="chave">Chave (key) única da Url para recuperação.</param>
        /// <returns>Objetvo do roteamento preenchido, se existir.</returns>
        public UrlDTO ObterRotaUrl(string chave)
        {
            
            UrlDTO dto = new UrlDTO();

            if (string.IsNullOrEmpty(chave))
                throw new NegocioException("Chave do roteamento não informando.");

            RotaUrlModel model = repositorios.RotasUrl().Obter(w => w.Chave.Equals(chave, StringComparison.OrdinalIgnoreCase));
            dto = UrlDTO.ConvertToDto(model);
            
            return dto;

        }

        /// <summary>
        /// Recupera a lista de Rotas de Url.
        /// </summary>
        /// <param name="hasVigentes">Flag para recuperar somente as vigêntes.</param>
        /// <returns>Lista[Objeto] de rotas de Urls conforme filtro informado.</returns>
        public List<UrlDTO> ListarRotasUrl(bool hasVigentes)
        {
            List<UrlDTO> listResult = new List<UrlDTO>();
            List<RotaUrlModel> listModels = null;

            if (hasVigentes) 
            {
                DateTime dataCorrente = DateTime.Now.Date;
                listModels = repositorios.RotasUrl().Listar(w => w.DataInicialVigencia >= dataCorrente && (w.DataFinalVigencia == null || w.DataFinalVigencia <= dataCorrente));
            } 
            else
            {
                listModels = repositorios.RotasUrl().Listar();
            }

            listResult = UrlDTO.ConvertToDto(listModels);
            return listResult;

        }

        /// <summary>
        /// Registra histórico de acesso a Rota de Url para metrica e controle de acessos.
        /// </summary>
        /// <param name="historicoAcesso">Objeto com as informações do historico de acesso.</param>
        public void RegistrarAcessoRotaUrl(HistoricoAcessoDTO historicoAcesso)
        {

            if (historicoAcesso == null)
                return; //Não registra histórico.
            if (string.IsNullOrEmpty(historicoAcesso.ChaveRota))
                throw new NegocioException("Chave do roteamento não informando.");
            if (string.IsNullOrEmpty(historicoAcesso.IpOrigemAcesso))
                throw new NegocioException("IP de origem do acesso não informando.");
            if (string.IsNullOrEmpty(historicoAcesso.InformacoesOrigemAcesso))
                throw new NegocioException("Informações de origem do acesso não informando.");

            //Verificar se a chave existe e está ativa.
            UrlDTO url = ObterRotaUrl(historicoAcesso.ChaveRota);
            if (url == null || !url.HasRotaVigente)
                throw new NegocioException($"Chave [{historicoAcesso.ChaveRota}] não existe ou não está mais vigente.");

            HistoricoAcessoRotaUrlModel historico = new HistoricoAcessoRotaUrlModel();
            historico.ChaveRota = url.Chave;
            historico.IpOrigemAcesso = historicoAcesso.IpOrigemAcesso;
            historico.HasDispositivoMovel = historicoAcesso.HasDispositivoMovel;
            historico.SistemaOperacionalAcesso = historicoAcesso.SistemaOperacionalAcesso;
            historico.InformacoesOrigemAcesso = historicoAcesso.InformacoesOrigemAcesso;
            historico.DataHoraAcesso = historicoAcesso.DataHoraAcesso;

            repositorios.HistoricoAcessos().Incluir(historico);

        }

        /// <summary>
        /// Lista histórico de acessos a uma Rota de Url pela sua chave.
        /// </summary>
        /// <param name="chaveRota">Chave da rota de Url para listagem do histórico.</param>
        /// <returns></returns>
        public List<HistoricoAcessoDTO> ListarHistoricoAcessoRotaUrl(string chaveRota)
        {
            
            if (string.IsNullOrEmpty(chaveRota))
                throw new NegocioException("Chave do roteamento não informando.");

            List<HistoricoAcessoDTO> listResult = new List<HistoricoAcessoDTO>();
            var listModels = repositorios.HistoricoAcessos().Listar(w => w.ChaveRota.Equals(chaveRota, StringComparison.OrdinalIgnoreCase));
            listResult = HistoricoAcessoDTO.ConvertToDto(listModels);
            return listResult;

        }

    }
}
