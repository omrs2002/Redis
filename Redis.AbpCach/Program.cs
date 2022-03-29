
using Autofac.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Caching;
using static System.Net.Mime.MediaTypeNames;

namespace Redis.AbpCach
{
    class Program
    {
        static async Task Main(string[] args)
        {

            using var application = AbpApplicationFactory.Create<AppModule>
            (builder =>
            {
                builder.UseAutofac(); //Autofac integration

                builder.Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = "127.0.0.1:6300";
                    options.InstanceName = "SampleInstance";
                });

                builder.Services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());


            });

            application.Initialize();

 await ReadDataFromRedis(application);

         

            //Console.ReadLine();
        }

        private static async Task ReadDataFromRedis(IAbpApplicationWithInternalServiceProvider app)
        {
            Guid g1Book1 = Guid.Parse("c4822c72-bd2b-4599-bf77-cbd253489216");
            var book_service = app.ServiceProvider.GetService<RedisBookService>();

            if (book_service != null)
            {

                //Thread.Sleep(TimeSpan.FromSeconds(5));
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Reading data from cache");

                var res1 = await book_service.GetAsync(g1Book1);

                if (res1 != null)
                    Console.WriteLine($"Reading data, book name: {res1.Name},book price : {res1.Price}  from cache");
                else
                {
                    Console.WriteLine("data not found! expired");
                    Console.WriteLine("try to get data with (GetOrAddAsync) func...");
                    res1 = await book_service.GetOrAddAsync(g1Book1);
                    Console.WriteLine($"Reading data, book name: {res1.Name},book price : {res1.Price}  from cache");
                }

                Console.WriteLine("Press ENTER to stop application...or (R) to retry");
                string? r = Console.ReadLine();

                if (r != null && r.ToLower().Equals("r"))
                {
                    await ReadDataFromRedis(app);
                }

            }

        }
    }

}
