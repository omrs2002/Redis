using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;

namespace Redis.AbpCach
{

    public class RedisBookService : ITransientDependency
    {
        private readonly  IDistributedCache<BookCacheItem> _cache;

        public RedisBookService(IDistributedCache<BookCacheItem> cache)
        {
            _cache = cache;
        }

        public async Task<BookCacheItem> GetAsync(Guid bookId)
        {

            return await _cache.GetAsync(bookId.ToString() //Cache key
            );
        }

        public async Task<BookCacheItem> GetOrAddAsync(Guid bookId)
        {
            return await _cache.GetOrAddAsync(
                bookId.ToString(), //Cache key
                async () => await GetBookFromDatabaseAsync(bookId),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1)
                }
            );
        }

        private Task<BookCacheItem> GetBookFromDatabaseAsync(Guid bookId)
        {
            var boook = new BookCacheItem { Name ="Book1" , Price = 90};
            Console.WriteLine("getting data from DB ...!");
            Thread.Sleep(1000);
            //TODO: get from database
            return Task.FromResult(boook);

        }
    }

}
