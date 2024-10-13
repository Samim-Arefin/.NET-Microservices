using Catalog.API.Entities;
using Catalog.API.Infrastructure;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public interface IDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }

    public class DbContext : IDbContext
    {
        private readonly IMongoDatabase _database;
        public DbContext()
        {
            var client =  new MongoClient(AppSettings.Settings.ConnectionStrings);
            _database = client.GetDatabase(AppSettings.Settings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name) => 
            _database.GetCollection<T>(name);
    }
}
