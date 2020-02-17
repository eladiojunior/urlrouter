using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlRouter.WebApi.Contexto.Entites;

namespace UrlRouter.WebApi.Contexto.Repositores
{
    public interface IRotaUrlRepository
    {
        Task<IEnumerable<RotaUrlEntity>> GetAll(string nome, bool hasVigente);
        Task<RotaUrlEntity> GetById(string id);
        Task<RotaUrlEntity> GetByChave(string chave);
        Task Create(RotaUrlEntity entity);
        Task<bool> Update(RotaUrlEntity entity);
        Task<bool> Delete(string id);
    }

    public class RotaUrlRepository : IRotaUrlRepository
    {
        private readonly IApplicationDbContext _context;
        public RotaUrlRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(RotaUrlEntity entity)
        {
            entity.DataInicialVigencia = DateTime.Now;
            await _context.RotaUrl.InsertOneAsync(entity);
        }

        public async Task<bool> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;
            ObjectId objectId = new ObjectId(id);
            FilterDefinition<RotaUrlEntity> filter = Builders<RotaUrlEntity>.Filter.Eq(m => m.Id, objectId);
            DeleteResult deleteResult = await _context.RotaUrl.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public Task<RotaUrlEntity> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            ObjectId objectId = new ObjectId(id);
            FilterDefinition<RotaUrlEntity> filter = Builders<RotaUrlEntity>.Filter.Eq(m => m.Id, objectId);
            return _context.RotaUrl.Find(filter).FirstOrDefaultAsync();
        }

        public Task<RotaUrlEntity> GetByChave(string chave)
        {
            if (string.IsNullOrEmpty(chave))
                return null;
            FilterDefinition<RotaUrlEntity> filter = Builders<RotaUrlEntity>.Filter.Eq(m => m.Chave, chave);
            return _context.RotaUrl.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RotaUrlEntity>> GetAll(string nome, bool hasVigente)
        {
            if (string.IsNullOrEmpty(nome) && hasVigente == false)
                return await _context.RotaUrl.Find(_ => true).ToListAsync();
            DateTimeOffset dataCorrente = DateTimeOffset.Now.Date;
            FilterDefinition<RotaUrlEntity> filter = null;
            if (string.IsNullOrEmpty(nome) && hasVigente == true)
                filter = Builders<RotaUrlEntity>.Filter.Where(w => w.DataInicialVigencia >= dataCorrente && (w.DataFinalVigencia == null || w.DataFinalVigencia <= dataCorrente));
            else
                filter = Builders<RotaUrlEntity>.Filter.Where(w => w.Nome.Contains(nome));
            return await _context.RotaUrl.Find(filter).ToListAsync();
        }

        public async Task<bool> Update(RotaUrlEntity entity)
        {
            if (entity == null || entity.Id == null)
                return false;
            ReplaceOneResult updateResult = await _context.RotaUrl.ReplaceOneAsync(filter: g => g.Id == entity.Id, replacement: entity);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
