using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UrlRouter.WebApi.Contexto.Entites;

namespace UrlRouter.WebApi.Contexto
{
    public class ApplicationDbContext : IApplicationDbContext
    {
        private readonly IMongoDatabase _db;
        public ApplicationDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("MongoDb:ConnectionString").Value);
            _db = client.GetDatabase(configuration.GetSection("MongoDb:Database").Value);
        }
        public IMongoCollection<RotaUrlEntity> RotaUrl => _db.GetCollection<RotaUrlEntity>("RotaUrl");
        public IMongoCollection<AcessoRotaUrlEntity> AcessoRotaUrl => _db.GetCollection<AcessoRotaUrlEntity>("AcessoRotaUrl");
    }

    public interface IApplicationDbContext
    {
        IMongoCollection<RotaUrlEntity> RotaUrl { get; }
        IMongoCollection<AcessoRotaUrlEntity> AcessoRotaUrl { get; }
    }
}
