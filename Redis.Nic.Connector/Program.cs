using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Redis.Nic.Connector.Redis;
using RedisConnector;


namespace Redis.Nic.Connector
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            IConfigurationBuilder builderJson = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot configurationJson = builderJson.Build();

            var builder = new HostBuilder()
                     .ConfigureServices(ser =>
                     {
                         
                         
                         
                         ser.Configure<RedisConfiguration>(configurationJson.GetSection("RedisConfiguration"));
                         ser.AddScoped<IRedisCash, RedisCach>();
                         ser.RegisterRedisConnector();
                         

                     })
            .Build();

            var cash = builder.Services.GetService<IRedisCash>();
            bool res = cash.SetData<string>("InvestorPortalStream_testRedisFromOmar", "testRedisFromOmarValue");
            var testdata = cash.GetData<string>("InvestorPortalStream_testRedisFromOmar");
            Console.WriteLine("InvestorPortalStream_testRedisFromOmar:"+testdata);
            Console.ReadLine();

            //test:

            
        }
    }
}