using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrlRouter.WebApi.Contexto.Entites;

namespace UrlRouter.WebApi.Contexto.Repositores
{
    public interface IAcessoRotaUrlRepository
    {
        Task<IEnumerable<AcessoRotaUrlEntity>> GetAll(string chave);
        Task<AcessoRotaUrlEntity> Get(string id);
        Task Create(AcessoRotaUrlEntity entity);
    }

    public class AcessoRotaUrlRepository : IAcessoRotaUrlRepository
    {
        private readonly IApplicationDbContext _context;
        public AcessoRotaUrlRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(AcessoRotaUrlEntity entity)
        {
            entity.DataHoraAcesso = DateTimeOffset.Now;
            await _context.AcessoRotaUrl.InsertOneAsync(entity);
        }

        public Task<AcessoRotaUrlEntity> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            ObjectId objectId = new ObjectId(id);
            FilterDefinition<AcessoRotaUrlEntity> filter = Builders<AcessoRotaUrlEntity>.Filter.Eq(m => m.Id, objectId);
            return _context.AcessoRotaUrl.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AcessoRotaUrlEntity>> GetAll(string chave)
        {
            if (string.IsNullOrEmpty(chave))
                return null;
            
            FilterDefinition<AcessoRotaUrlEntity> filter = Builders<AcessoRotaUrlEntity>.Filter.Eq(m => m.ChaveRota, chave);
            return await _context.AcessoRotaUrl.Find(filter).ToListAsync();
        }

    }
}
