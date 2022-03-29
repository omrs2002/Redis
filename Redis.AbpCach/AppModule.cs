using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Redis.AbpCach
{
    //Add dependency to the AbpAutofacModule
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpCachingModule))]
    [DependsOn(typeof(AbpCachingStackExchangeRedisModule))]
    public class AppModule : AbpModule
    {


        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDistributedCacheOptions>(options =>
            {options.KeyPrefix = "RedisAbpCach";});

            Configure<RedisCacheOptions>(options =>
            {options.Configuration = "127.0.0.1:6300";});
        }
    }
}
