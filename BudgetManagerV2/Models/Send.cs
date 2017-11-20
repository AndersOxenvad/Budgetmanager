using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;

namespace BudgetManagerV2.Models
{
    public class Send
    {
        public void Main(JObject confirmation)
        {
            var factory = new ConnectionFactory() { HostName = "amqp://1doFhxuC:WGgk9kXy_wFIFEO0gwB_JiDuZm2-PrlO@black-ragwort-810.bigwig.lshift.net:10802/SDU53lDhKShK" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "Rapid", type: "direct");
                byte[] data;
                BinaryFormatter bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, confirmation);
                    data = ms.ToArray();
                      
                }
                var body = data;
                    channel.BasicPublish(exchange: "direct_logs", routingKey: "lascreatetransactionconfirmation", basicProperties: null, body: body);
            }
        }
    }
}