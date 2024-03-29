﻿using Redis.ConsoleApp.RedisStream;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Redis.ConsoleApp
{
    class Program
    {
        private static void TestReadWrite()
        {
            Console.WriteLine("Saving random data in cache");
            SaveBigData();
            Console.WriteLine("Reading data from cache");
            ReadData();
        }
        private async static Task<bool> TestStreaming()
        {
            
            using (RedisStreaming rs = new())
            {
                string msg_id =await  rs.Write();
                rs.Read(msg_id);
                return true;
            } ;
        }
        private static void TestConsuming(string chanel_name)
        {

            using var rds = RedisConnectorHelper.Connection;

            ISubscriber sub = rds.GetSubscriber();
            //Subscribe to the channel named messages
            sub.Subscribe(chanel_name, (channel, message) =>
            {
                //Output received message
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
            });
            Console.WriteLine("subscribed messages");
            Console.ReadKey();


        }


        static void Main(string[] args)
        {

            Console.WriteLine("(0) - ping DB");
            Console.WriteLine("(1) - Test Read Write:");
            Console.WriteLine("(2) - Test Serialize Deserialize:");
            Console.WriteLine("(3) - Test Streaming:");
            Console.WriteLine("(4) - Test Consuming:");

            var input = Console.ReadLine();
            switch (input)
            {
                case "0":
                    PingDB();
                    break;
                case "1":
                    TestReadWrite();
                    break;
                case "2":
                    TstSerializeDeserialize();
                    break;
                case "3":
                    _ = TestStreaming();
                    break;
                case "4":
                    Console.WriteLine("Enter chanel name:");
                    string ch_name = Console.ReadLine();
                    TestConsuming(ch_name);
                    break;
                default:
                    break;
            }
            
            Console.WriteLine("Again (Y/N):");
            var Again = Console.ReadLine();
            if (Again.ToUpper() == "Y")
            {
                Main(args);
            }
            else
            {
                Environment.Exit(0);
            }



        }

        private static void PingDB()
        {
            Console.WriteLine("Pinging  DB...");
            var png = RedisConnectorHelper.Connection.GetDatabase().Ping();
            Console.WriteLine("DB Pinged  successfully");

        }

        private static void TstSerializeDeserialize()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Saving Employee");
            var emp = new Employee { ID = 1, EmpName = "Ahmad" };
            var ser_emp = System.Text.Json.JsonSerializer.Serialize(emp);
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            cache.StringSet("emp:" + emp.ID, ser_emp);
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Gt Employee from cash");

            var value = cache.StringGet("emp:" + emp.ID);
            Employee DesEmp = System.Text.Json.JsonSerializer.Deserialize<Employee>(value);
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Print Employee");
            Console.WriteLine($"emp name {DesEmp.EmpName }");
        }

        public static void SaveBigData()
        {
            var devicesCount = 10;
            var rnd = new Random();
            var cache = RedisConnectorHelper.Connection.GetDatabase();

            for (int i = 1; i < devicesCount; i++)
            {
                var value = rnd.Next(0, 100);
                cache.StringSet($"Device_Status:{i}", value);
            }
        }

        public static void ReadData()
        {
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            var devicesCount = 10;
            for (int i = 1; i < devicesCount; i++)
            {

                var value = cache.StringGet($"Device_Status:{i}");
                Console.WriteLine($"Valor={value}");
            }
        }


    }
}
