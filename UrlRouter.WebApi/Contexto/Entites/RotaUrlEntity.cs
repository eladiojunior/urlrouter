using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlRouter.WebApi.Contexto.Entites
{
    public class RotaUrlEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("chave_rota")]
        public string Chave { get; set; }

        [BsonElement("nome_rota")]
        public string Nome { get; set; }

        [BsonElement("descricao_rota")]
        public string Descricao { get; set; }
        
        [BsonElement("url_destino")]
        public string UrlDestino { get; set; }
        
        [BsonElement("url_destino_ios")]
        public string UrlDestinoIOS { get; set; }
        
        [BsonElement("url_destino_android")]
        public string UrlDestinoAndroid { get; set; }
        
        [BsonElement("url_destino_windowsphone")]
        public string UrlDestinoWindowsPhone { get; set; }
        
        [BsonElement("has_controle_acesso")]
        public bool HasControleAcesso { get; set; }

        [BsonElement("data_inicial_vigencia")]
        public DateTimeOffset DataInicialVigencia { get; set; }

        [BsonElement("data_final_vigencia")]
        public DateTimeOffset? DataFinalVigencia { get; set; }

    }
}
