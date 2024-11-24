using AutoMapper;
using Catalog.API.Data;
using Catalog.API.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.API.Response;
using System.Linq.Expressions;
using System.Net;

namespace Catalog.API.Services.Implementation
{
    public class CommonService<T> : ICommonService<T> where T : class
    {
        private readonly IDbContext _context;
        private readonly IMongoCollection<T> _collection;
        private readonly IMapper _mapper;

        public CommonService(IDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _collection = _context.GetCollection<T>(typeof(T).Name);
        }

        public async Task<U?> GetByIdAsync<U>(string id, CancellationToken cancellationToken = default)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<T>.Filter.Eq("_id", objectId);
            var value = await (await QueryAsync(filter, cancellationToken)).FirstOrDefaultAsync(cancellationToken);
            return _mapper.Map<U>(value);
        }

        public async Task<U?> FindOneAsync<U>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Where(expression);
            var value = await (await QueryAsync(filter, cancellationToken)).FirstOrDefaultAsync(cancellationToken);
            return _mapper.Map<U>(value);
        }

        public virtual async Task<List<U>?> GetAllAsync<U>(Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default)
        {
            var filter = expression is not null ? Builders<T>.Filter.Where(expression) : Builders<T>.Filter.Where(_ => true);
            var value = await (await QueryAsync(filter, cancellationToken)).ToListAsync(cancellationToken);
            return _mapper.Map<List<U>>(value);
        }

        public async Task<U?> InsertOneAsync<U>(U uEntity, T? tEntity = null, CancellationToken cancellationToken = default)
        {
            var mappedValue = _mapper.Map<T>(uEntity);
            await _collection.InsertOneAsync(mappedValue, null, cancellationToken);
            return _mapper.Map<U>(mappedValue);
        }

        public async Task<Unit> ReplaceOneAsync<U>(string id, U uEntity, T? tEntity = null, CancellationToken cancellationToken = default)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<T>.Filter.Eq("_id", objectId);
            var value = await (await QueryAsync(filter, cancellationToken)).FirstOrDefaultAsync(cancellationToken);

            if (value is null)
            {
                throw new BadHttpRequestException($"{id} not found!");
            }

            var mappedValue = _mapper.Map<U, T>(uEntity, value);

            await _collection.ReplaceOneAsync(filter, mappedValue, options: new ReplaceOptions(), cancellationToken);

            return new(statusCode: (int)HttpStatusCode.OK, isSuccess: true, message: "Successfully updated!");
        }

        public async Task<Unit> DeleteOneAsync(string id, CancellationToken cancellationToken = default)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<T>.Filter.Eq("_id", objectId);
            var value = await (await QueryAsync(filter, cancellationToken)).FirstOrDefaultAsync(cancellationToken);

            if (value is null)
            {
                throw new BadHttpRequestException($"{id} not found!");
            }

            await _collection.DeleteOneAsync(filter, null, cancellationToken);

            return new(statusCode: (int)HttpStatusCode.OK, isSuccess: true, message: "Successfully deleted!");
        }

        public virtual async Task<Unit> DeleteManyAsync(Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default)
        {
            var filter = expression is not null ? Builders<T>.Filter.Where(expression) : Builders<T>.Filter.Where(_ => true);
            await _collection.DeleteManyAsync(filter, null, cancellationToken);
            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Successfully deleted!"
            };
        }

        protected async Task<IAsyncCursor<T>> QueryAsync(FilterDefinition<T> filter, CancellationToken cancellationToken)
        {
            return await _collection.FindAsync<T>(filter, null, cancellationToken);
        }
    }
}
