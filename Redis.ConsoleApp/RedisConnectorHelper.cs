using StackExchange.Redis;
using System;

namespace Redis.ConsoleApp
{
    public class RedisConnectorHelper
    {   
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        static RedisConnectorHelper()
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { "127.0.0.1:6379" }
            };

            
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(configurationOptions);
            });
        }

    

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }

}
