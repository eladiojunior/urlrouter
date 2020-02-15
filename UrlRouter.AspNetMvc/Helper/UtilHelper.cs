using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace UrlRouter.AspNetMvc.Helper
{
    public class UtilHelper
    {
        private static List<SistemaOperacional> listaSoYmal = null;
        private static List<DeviceMobile> listaDeviceYmal = null;

        /// <summary>
        /// Obtem o sistema operacional utilizado pelo Client (HTTP) de dentro do USER-AGENT (HTTP). 
        /// </summary>
        /// <param name="userAgent">Informações do USER-AGENT do protocolo HTTP.</param>
        /// <returns>Informação do sistema operacional utilizado, caso não encontre, retorna: Não detectado.</returns>
        public static string ObterSistemaOperacional(string userAgent)
        {

            string so = "Não detectado";
            if (string.IsNullOrEmpty(userAgent))
                return so;

            if (listaSoYmal == null)
                CarregarOss();

            foreach (var item in listaSoYmal)
            {
                Regex regex = new Regex(item.Regex);
                Match match = regex.Match(userAgent);
                if (match.Success)
                {
                    so = AjustarModeloGroups(item.Name, match.Groups);
                    if (!string.IsNullOrEmpty(item.Version))
                    {
                        string versao = AjustarModeloGroups(item.Version, match.Groups);
                        if (!string.IsNullOrEmpty(versao))
                            so += $" [{versao}]";
                    }
                    break;
                }
            }

            return so;

        }

        /// <summary>
        /// Carregar os SO (Sistemas Operacionais) de um arquivo em formato Yaml.
        /// </summary>
        private static void CarregarOss()
        {
            var assembly = typeof(UtilHelper).GetTypeInfo().Assembly;
            Stream resource = assembly.GetManifestResourceStream("UrlRouter.AspNetMvc.Helper.regexes.oss.yml");
            using (var reader = new StreamReader(resource))
            {
                listaSoYmal = new Deserializer().Deserialize<List<SistemaOperacional>>(reader);
            }
        }

        /// <summary>
        /// Obtem da Request do usuário o IP (hostname) do cliente.
        /// </summary>
        /// <param name="request">Informações da requisição HTTP Request do Cliente.</param>
        /// <returns></returns>
        public static string ObterIpMaquinaCliente(HttpRequest request)
        {
            string ipClient = string.Empty;
            if (request == null)
                return ipClient;
            ipClient = request.Headers["HTTP_X_FORWARDED_FOR"].ToString();
            if (string.IsNullOrEmpty(ipClient))
            {//Verificar em outra variável.
                ipClient = request.Headers["REMOTE_ADDR"].ToString();
            }
            if (string.IsNullOrEmpty(ipClient))
            {//Verificar em outra variável.
                ipClient = request.Headers["HTTP_HOST"].ToString();
            }
            return ipClient;
        }

        /// <summary>
        /// Verifica se dentro do USER-AGENT existe um dispositivo móvel. 
        /// </summary>
        /// <param name="userAgent">Informações do USER-AGENT do protocolo HTTP.</param>
        /// <returns>Existe ou não um dispositivo móvel.</returns>
        public static bool HasDeviceMobile(string userAgent)
        {
            bool hasDevice = false;
            string[] listDevicesMobile = { "feature phone", "smartphone", "tablet" };
            DeviceMobile device = ObterDeviceMobile(userAgent);
            if (device == null)
                return hasDevice;
            hasDevice = listDevicesMobile.Any(x => x.Equals(device.Device.ToLower()));
            return hasDevice;
        }

        /// <summary>
        /// Obtem se dentro do USER-AGENT existe um dispositivo móvel. 
        /// </summary>
        /// <param name="userAgent">Informações do USER-AGENT do protocolo HTTP.</param>
        /// <returns>Existindo retorna nas informações do dispositivo, caso não retorna NULL.</returns>
        public static DeviceMobile ObterDeviceMobile(string userAgent)
        {

            if (string.IsNullOrEmpty(userAgent))
                return null;

            if (listaDeviceYmal == null)
                CarregarDevicesMobiles();

            //Verificar se é um dispositivo.
            foreach (var item in listaDeviceYmal)
            {
                if (VerificarDeviceByRegex(item.Regex, userAgent, out GroupCollection groups))
                {
                    if (!string.IsNullOrEmpty(item.Model))
                        item.Model = AjustarModeloGroups(item.Model, groups);
                    else if (item.Models != null)
                    {//Verificar o modelo do Device.
                        foreach (var model in item.Models)
                        {
                            if (VerificarDeviceByRegex(model.Regex, userAgent, out groups))
                            {
                                if (!string.IsNullOrEmpty(model.Model))
                                    item.Model = AjustarModeloGroups(model.Model, groups);
                                if (!string.IsNullOrEmpty(model.Device))
                                    item.Device = model.Device;
                                break;
                            }
                        }
                    }
                    return item;
                }
            }
            return null;

        }

        /// <summary>
        /// Verificar um device dentro do USER-AGENT (HTTP) a partir do regex de dispositivos móveis.
        /// </summary>
        /// <param name="strRegex">Expressão regex de verificação do dispositivo.</param>
        /// <param name="userAgent">Informações do USER-AGENT do protocolo HTTP.</param>
        /// <param name="groups">Objeto com os groups do regex, quando encontrado.</param>
        private static bool VerificarDeviceByRegex(string strRegex, string userAgent, out GroupCollection groups)
        {
            try
            {
                Regex regex = new Regex(strRegex);
                Match match = regex.Match(userAgent);
                if (match.Success)
                {
                    groups = match.Groups;
                    return true;
                }
            }
            catch (System.Exception)
            {//Rejeitar..
                Debug.WriteLine($"Erro Regex: >>[{strRegex}]<<");
            }
            groups = null;
            return false;
        }

        /// <summary>
        /// Ajusta a informação do modelo (model) do dispositivo (device).
        /// </summary>
        /// <param name="model">Modelo do dispositivo.</param>
        /// <param name="groups">Grupo com os parametros encontrados no regex.</param>
        /// <returns></returns>
        private static string AjustarModeloGroups(string model, GroupCollection groups)
        {
            if (string.IsNullOrEmpty(model))
                return string.Empty;

            if (groups == null || groups.Count == 0)
                return model.Trim();

            for (int i = 0; i < groups.Count; i++)
            {
                string key = $"${i}";
                if (model.Contains(key))
                {
                    string strModelo = groups[i].Value;
                    model = model.Replace(key, strModelo);
                }
            }
            return model.Trim();
        }

        /// <summary>
        /// Carregar os devices de mobile de um arquivo em formato Yaml.
        /// </summary>
        private static void CarregarDevicesMobiles()
        {
            var assembly = typeof(UtilHelper).GetTypeInfo().Assembly;
            Stream resource = assembly.GetManifestResourceStream("UrlRouter.AspNetMvc.Helper.regexes.mobiles.yml");
            using (var reader = new StreamReader(resource))
            {
                var yaml = new YamlStream();
                yaml.Load(reader);
                listaDeviceYmal = new List<DeviceMobile>();
                var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
                foreach (var entry in mapping.Children)
                {
                    string brand = ((YamlScalarNode)entry.Key).Value;
                    var nodes = ((YamlMappingNode)entry.Value).Children;
                    DeviceMobile device = new DeviceMobile();
                    device.Brand = brand;
                    if (nodes.ContainsKey("device"))
                        device.Device = ((YamlScalarNode)nodes["device"]).Value;
                    if (nodes.ContainsKey("regex"))
                        device.Regex = ((YamlScalarNode)nodes["regex"]).Value;
                    if (nodes.ContainsKey("model"))
                        device.Model = ((YamlScalarNode)nodes["model"]).Value;
                    if (nodes.ContainsKey("models"))
                    {//Verificar se existem models... se sim, carregar todos.
                        var models = ((YamlSequenceNode)nodes["models"]).Children;
                        device.Models = new List<ModelDevice>();
                        foreach (YamlMappingNode model in models)
                        {
                            ModelDevice modelDevice = new ModelDevice();
                            if (model.Children.ContainsKey("regex"))
                                modelDevice.Regex = ((YamlScalarNode)model.Children["regex"]).Value;
                            if (model.Children.ContainsKey("model"))
                                modelDevice.Model = ((YamlScalarNode)model.Children["model"]).Value;
                            if (model.Children.ContainsKey("device"))
                                modelDevice.Device = ((YamlScalarNode)model.Children["device"]).Value;
                            device.Models.Add(modelDevice);
                        }
                    }
                    listaDeviceYmal.Add(device);
                }
            }
        }
    }

    internal class SistemaOperacional
    {
        [YamlMember(Alias = "regex")]
        public string Regex { get; set; }
        [YamlMember(Alias = "name")]
        public string Name { get; set; }
        [YamlMember(Alias = "version")]
        public string Version { get; set; }
    }

    public class DeviceMobile
    {
        public string Brand { get; set; }
        internal string Regex { get; set; }
        public string Device { get; set; }
        public string Model { get; set; }
        internal List<ModelDevice> Models { get; set; }
    }
    public class ModelDevice
    {
        internal string Regex { get; set; }
        public string Device { get; set; }
        public string Model { get; set; }
    }
}
