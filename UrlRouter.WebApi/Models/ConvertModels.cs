using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlRouter.WebApi.Contexto.Entites;

namespace UrlRouter.WebApi.Models
{
    public class ConvertModels
    {
        internal static IEnumerable<RotaUrlModel> ConverterToListModel(IEnumerable<RotaUrlEntity> entites)
        {
            List<RotaUrlModel> model = new List<RotaUrlModel>();
            if (entites == null)
                return model;
            foreach (var item in entites)
            {
                model.Add(ConverterToModel(item));
            }
            return model;
        }

        internal static RotaUrlModel ConverterToModel(RotaUrlEntity entity)
        {
            if (entity == null)
                return null;

            RotaUrlModel model = new RotaUrlModel();
            model.Id = entity.Id.ToString();
            model.Chave = entity.Chave;
            model.Nome = entity.Nome;
            model.Descricao = entity.Descricao;
            model.UrlDestino = entity.UrlDestino;
            model.UrlDestinoIOS = entity.UrlDestinoIOS;
            model.UrlDestinoAndroid = entity.UrlDestinoAndroid;
            model.UrlDestinoWindowsPhone = entity.UrlDestinoWindowsPhone;
            model.HasControleAcesso = entity.HasControleAcesso;
            model.DataInicialVigencia = entity.DataInicialVigencia;
            model.DataFinalVigencia = entity.DataFinalVigencia;
            return model;
        }

        internal static RotaUrlEntity ConverterToEntity(RotaUrlModel model)
        {
            if (model == null)
                return null;
            RotaUrlEntity entity = new RotaUrlEntity();
            if (!string.IsNullOrEmpty(model.Id))
                entity.Id = new MongoDB.Bson.ObjectId(model.Id);
            entity.Chave = model.Chave;
            entity.Nome = model.Nome;
            entity.Descricao = model.Descricao;
            entity.UrlDestino = model.UrlDestino;
            entity.UrlDestinoIOS = model.UrlDestinoIOS;
            entity.UrlDestinoAndroid = model.UrlDestinoAndroid;
            entity.UrlDestinoWindowsPhone = model.UrlDestinoWindowsPhone;
            entity.HasControleAcesso = model.HasControleAcesso;
            entity.DataInicialVigencia = model.DataInicialVigencia;
            entity.DataFinalVigencia = model.DataFinalVigencia;
            return entity;
        }

        internal static IEnumerable<AcessoRotaUrlModel> ConverterToListModel(IEnumerable<AcessoRotaUrlEntity> entites)
        {
            List<AcessoRotaUrlModel> model = new List<AcessoRotaUrlModel>();
            if (entites == null)
                return model;
            foreach (var item in entites)
            {
                model.Add(ConverterToModel(item));
            }
            return model;
        }

        internal static AcessoRotaUrlModel ConverterToModel(AcessoRotaUrlEntity entity)
        {
            if (entity == null)
                return null;

            AcessoRotaUrlModel model = new AcessoRotaUrlModel();
            model.Id = entity.Id.ToString();
            model.ChaveRota = entity.ChaveRota;
            model.IpOrigemAcesso = entity.IpOrigemAcesso;
            model.HasDispositivoMovel = entity.HasDispositivoMovel;
            model.TipoDispositivoMovel = entity.TipoDispositivoMovel;
            model.ModeloDispositivoMovel = entity.ModeloDispositivoMovel;
            model.SistemaOperacionalAcesso = entity.SistemaOperacionalAcesso;
            model.InformacoesOrigemAcesso = entity.InformacoesOrigemAcesso;
            model.DataHoraAcesso = entity.DataHoraAcesso;
            return model;
        }

        internal static AcessoRotaUrlEntity ConverterToEntity(AcessoRotaUrlModel model)
        {
            if (model == null)
                return null;
            AcessoRotaUrlEntity entity = new AcessoRotaUrlEntity();
            if (!string.IsNullOrEmpty(model.Id))
                entity.Id = new MongoDB.Bson.ObjectId(model.Id);
            entity.ChaveRota = model.ChaveRota;
            entity.IpOrigemAcesso = model.IpOrigemAcesso;
            entity.HasDispositivoMovel = model.HasDispositivoMovel;
            entity.TipoDispositivoMovel = model.TipoDispositivoMovel;
            entity.ModeloDispositivoMovel = model.ModeloDispositivoMovel;
            entity.SistemaOperacionalAcesso = model.SistemaOperacionalAcesso;
            entity.InformacoesOrigemAcesso = model.InformacoesOrigemAcesso;
            entity.DataHoraAcesso = model.DataHoraAcesso;
            return entity;
        }
    }
}
