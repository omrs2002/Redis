using RedisConnector.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis.Nic.Connector.Redis
{
    internal class RedisCach : IRedisCash
    {

        private readonly IRedisConnector _redisConnector;

        public RedisCach(IRedisConnector redisConnector)
        {
            _redisConnector = redisConnector;
        }


        public T GetData<T>(string key)
        {
           var cacheData = _redisConnector.GetData<T>(key);
           return cacheData;
        }

        public bool RemoveData(string key)
        {
            _redisConnector.RemoveData(key);
            return true;    
        }

        public bool SetData<T>(string key, T value)
        {
            var expirationTime = DateTimeOffset.Now.AddHours(5);
            var msg = new RedisMessage("InvestorPortalStream", key,value?.ToString()+ "_AsStream");
            _redisConnector.StreamAddAsync(msg,false,true);
            return _redisConnector.SetData(key, value?.ToString()+" as data", expirationTime);
        }

       
    }
}
