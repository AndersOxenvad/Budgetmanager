using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Threading;

namespace BudgetManagerV2.Models
{
    public class Recieve
    {

        public static void Main()
        {
            new Thread(() =>
            {
                db database = new db();
                var factory = new ConnectionFactory() { HostName = "amqp://1doFhxuC:WGgk9kXy_wFIFEO0gwB_JiDuZm2-PrlO@black-ragwort-810.bigwig.lshift.net:10803/SDU53lDhKShK" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "Rapid", type: "direct");
                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: "lascreatetransaction");
                    channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: "lasupdatetransaction");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var data = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        if (routingKey == "lascreatetransaction")
                        {
                            database.CreateTransaction(data);
                        }
                        else if (routingKey == "lasupdatetransaction")
                        {
                            database.UpdateTransaction(data);
                        }

                    };
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                }


            }).Start();

        }


    }
}