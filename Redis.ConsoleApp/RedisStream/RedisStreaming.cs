using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis.ConsoleApp.RedisStream
{
    public class RedisStreaming : IDisposable
    {

        private IDatabase redisDB ;
        private const string streamID = "5abbcfc5-e407-4892-a0bb-e26e98bbd949";
        public RedisStreaming()
        {
            redisDB = RedisConnectorHelper.Connection.GetDatabase();
        }

        public void Dispose()
        {
            redisDB = null;
        }
  
        public bool CreateConsumerGroup(string grname)
        {
            try
            {
                return redisDB.StreamCreateConsumerGroup(streamID, grname, StreamPosition.NewMessages);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
           
        }
        public string GetStreamInfo(string str_name)
        {
            try
            {
                var info0 = redisDB.StreamInfo(str_name);
                var info = redisDB.StreamInfo(new RedisKey(str_name));
                return info.Length.ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
                return "";
            }
            
        }
        public async Task<string> Write()
        {
            var values = new NameValueEntry[]
            {
                new NameValueEntry("temp", "19.8"),
                new NameValueEntry("TEMP_DATE", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"))
            };
            ;//Guid.NewGuid().ToString();
            var messageId = await redisDB.StreamAddAsync(streamID, values);

            return messageId;
        }
  
        public void ShowPendingMsgInfo()
        {
            var pendingInfo = redisDB.StreamPending(streamID, "omar_group");

            Console.WriteLine(pendingInfo.PendingMessageCount);
            Console.WriteLine(pendingInfo.LowestPendingMessageId);
            Console.WriteLine(pendingInfo.HighestPendingMessageId);
            Console.WriteLine($"Consumer count: {pendingInfo.Consumers.Length}.");
            Console.WriteLine(pendingInfo.Consumers.First().Name);
            Console.WriteLine(pendingInfo.Consumers.First().PendingMessageCount);
        }
        public string Read(string msg_id)
        {
            try
            {
                ShowPendingMsgInfo();

                var messages = redisDB.StreamRead(streamID, "0-0");
                var curr_msg = messages.FirstOrDefault(f => f.Id ==new RedisValue(msg_id));
                var info = redisDB.StreamInfo(streamID);
                //var ackn = redisDB.StreamAcknowledge(streamID, "omar_group", msg_id, CommandFlags.FireAndForget);
                //var ackn2 = redisDB.StreamAcknowledge(streamID, "omar_group", new RedisValue(msg_id));
                return curr_msg.Values[1].Value.ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
                return "";
            }
            
        }
    }
}

