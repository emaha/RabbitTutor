using Common;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using MassTransit;
using RawRabbit;

namespace Sender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IBusControl _busControl;

        public ValuesController(IBusControl busControl)
        {
            _busControl = busControl;
        }

        [HttpPost("mass_event")]
        public async Task MassEvent(CancellationToken cancellationToken)
        {
            Console.WriteLine("TIME: " + DateTime.Now.ToString("O"));
            await _busControl.Publish(new FileReceivedEvent { Message = "ASD" }, cancellationToken);
            Console.WriteLine("TIME: " + DateTime.Now.ToString("O"));
        }

        [HttpPost("cmd")]
        public async Task Command(CancellationToken cancellationToken)
        {
            // Отправить сообщение прямо в очередь
            var endpoint = _busControl.GetSendEndpoint(new Uri("rabbitmq://192.168.0.192/Consumer1_Queue")).Result;
            await endpoint.Send(new FileReceivedEvent { Message = "FEWFE" }, cancellationToken);
        }

        #region Another

        [HttpPost("rabbit")]
        public async Task RabbitClient()
        {
            var factory = new ConnectionFactory() { HostName = "192.168.0.192" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var order = Order.Create();

                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Order));
                MemoryStream ms = new MemoryStream();

                jsonFormatter.WriteObject(ms, order);
                var data = ms.ToArray();

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: data);
                Console.WriteLine(" [x] Sent {0}", data.Length);
            }
        }

        [HttpPost("easy")]
        public async Task EasyClient()
        {
            var bus = RabbitHutch.CreateBus("host=192.168.0.192;publisherConfirms=true;timeout=10");

            var order = Order.Create();
            await bus.PublishAsync(order);

            Console.WriteLine("Sent");
        }

        #endregion Another
    }
}