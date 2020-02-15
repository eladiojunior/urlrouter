using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace UrlRouter.WebApi.Contexto.Entites
{
    public class AcessoRotaUrlEntity
    {
        public ObjectId Id { get; set; }
        [BsonElement("chave_rota")]
        public string ChaveRota { get; set; }
        [BsonElement("ip_origem_acesso")]
        public string IpOrigemAcesso { get; set; }
        [BsonElement("has_dispositivo_movel")]
        public bool HasDispositivoMovel { get; set; }
        [BsonElement("tipo_dispositivo_movel")]
        public string TipoDispositivoMovel { get; set; }
        [BsonElement("modelo_dispositivo_movel")]
        public string ModeloDispositivoMovel { get; set; }
        [BsonElement("sistema_operacional_acesso")]
        public string SistemaOperacionalAcesso { get; set; }
        [BsonElement("informacoes_origem_acesso")]
        public string InformacoesOrigemAcesso { get; set; }
        [BsonElement("datahora_acesso")]
        public DateTimeOffset DataHoraAcesso { get; set; }
    }
}
