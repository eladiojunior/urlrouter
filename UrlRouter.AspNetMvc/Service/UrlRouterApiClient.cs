using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private HttpClient _client;
        private IConfiguration _configuration;

        public UrlRouterApiClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _configuration = configuration;
        }

        /// <summary>
        /// Grava as informações de uma rota de Url.
        /// </summary>
        /// <param name="model">Informações da rota URL para gravação.</param>
        public void Gravar(RotaUrlModelApi model)
        {
            try
            {

                string baseURL = _configuration.GetSection("RotaUrlAPI:BaseURL").Value;
                string key = _configuration.GetSection("RotaUrlAPI:Key").Value;
                if (!string.IsNullOrEmpty(key))
                    baseURL += $"?api_key={key}";

                StringContent content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (string.IsNullOrEmpty(model.Id))
                    response = _client.PostAsync(baseURL, content).Result;
                else
                    response = _client.PutAsync(baseURL, content).Result;

                response.EnsureSuccessStatusCode();

            }
            catch (Exception erro)
            {
                throw new HttpRequestException("Erro ao gravar rota URL.", erro);
            }
        }

        /// <summary>
        /// Listar as rotas da URL, via API, conforme o parâmetro informado.
        /// </summary>
        /// <param name="nome">Filtro por nome ou parte do nome da rota para filtro (opcional).</param>
        /// <param name="hasVigentes">Flag para listar apenas as rotas vigêntes (defult: false).</param>
        /// <returns></returns>
        public ICollection<RotaUrlModelApi> Listar(string nome, bool hasVigentes)
        {
            try
            {
                string separadorBaseUrl = "?";
                string baseURL = _configuration.GetSection("RotaUrlAPI:BaseURL").Value;
                string key = _configuration.GetSection("RotaUrlAPI:Key").Value;
                if (!string.IsNullOrEmpty(key))
                    baseURL += $"?api_key={key}";
                
                if (baseURL.Contains("?")) separadorBaseUrl = "&";
                if (!string.IsNullOrEmpty(nome))
                    baseURL += $"{separadorBaseUrl}nome={nome}";
                baseURL += $"{separadorBaseUrl}hasVigentes={hasVigentes.ToString()}";

                var response = _client.GetAsync(baseURL).Result;

                response.EnsureSuccessStatusCode();
                string conteudo = response.Content.ReadAsStringAsync().Result;
                ICollection<RotaUrlModelApi> lista = JsonConvert.DeserializeObject<ICollection<RotaUrlModelApi>>(conteudo);
                return lista;

            }
            catch (Exception erro)
            {
                throw new HttpRequestException("Erro ao recuperar a lista de rotas URL.", erro);
            }
        }
        
        /// <summary>
        /// Recupera uma Rota URL pelo seu identificador.
        /// </summary>
        /// <param name="idRota">Identificador da rota URL.</param>
        /// <returns></returns>
        public RotaUrlModelApi Obter(string idRota)
        {
            try
            {

                string baseURL = _configuration.GetSection("RotaUrlAPI:BaseURL").Value;
                baseURL += $"{idRota}";
                string key = _configuration.GetSection("RotaUrlAPI:Key").Value;
                if (!string.IsNullOrEmpty(key))
                    baseURL += $"?api_key={key}";


                var response = _client.GetAsync(baseURL).Result;

                response.EnsureSuccessStatusCode();
                string conteudo = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<RotaUrlModelApi>(conteudo);
                return result;

            }
            catch (Exception erro)
            {
                throw new HttpRequestException($"Erro ao recuperar rota URL com id [{idRota}].", erro);
            }
        }

        /// <summary>
        /// Registra o acesso ao uma rota URL informando os dados de acesso do usuário.
        /// </summary>
        /// <param name="model">Informações dos dados de acesso a Rota URL para estatística.</param>
        public void RegistrarAcesso(AcessoRotaUrlModelApi model)
        {
            try
            {

                string baseURL = _configuration.GetSection("AcessoRotaUrlAPI:BaseURL").Value;
                string key = _configuration.GetSection("AcessoRotaUrlAPI:Key").Value;
                if (!string.IsNullOrEmpty(key))
                    baseURL += $"?api_key={key}";

                StringContent content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PostAsync(baseURL, content).Result;
                response.EnsureSuccessStatusCode();

            }
            catch (Exception erro)
            {
                throw new HttpRequestException("Erro ao registrar acesso a rota URL.", erro);
            }
        }

        /// <summary>
        /// Remove (logicamente) a rota URL pelo identificador.
        /// </summary>
        /// <param name="idRota">Identificador da rota Url para remoção lógica.</param>
        public void Remover(string idRota)
        {
            try
            {

                string baseURL = _configuration.GetSection("RotaUrlAPI:BaseURL").Value;
                baseURL += $"{idRota}";
                string key = _configuration.GetSection("RotaUrlAPI:Key").Value;
                if (!string.IsNullOrEmpty(key))
                    baseURL += $"?api_key={key}";

                var response = _client.DeleteAsync(baseURL).Result;
                response.EnsureSuccessStatusCode();

            }
            catch (Exception erro)
            {
                throw new HttpRequestException($"Erro ao remover a rota URL com id [{idRota}].", erro);
            }
        }
    }
}
