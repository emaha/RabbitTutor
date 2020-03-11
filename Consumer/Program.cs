using GreenPipes;
using MassTransit;
using System;

namespace Consumer
{
    internal class Program
    {
        private static Random r = new Random();

        private static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://192.168.139.102"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                // В эту очередь будут попадать сообщения, которые были посланы с RoutingKey = "routingKey"
                cfg.ReceiveEndpoint(host, "Consumer1_Queue", e =>
                {
                    //e.BindMessageExchanges = false;
                    e.Consumer(() => new FileReceivedConsumer(1));
                    //e.Bind("TestMessage", x =>
                    //{
                    //    x.ExchangeType = "direct";
                    //    x.RoutingKey = "routingKey";
                    //});
                });

                // В эту с RoutingKey = "routingKey2"
                //cfg.ReceiveEndpoint(host, "TestMessage_Queue2", e =>
                //{
                //    e.BindMessageExchanges = false;
                //    e.Consumer(() => new FileReceivedConsumer(2));
                //    e.Bind("TestMessage", x =>
                //    {
                //        x.ExchangeType = "direct";
                //        x.RoutingKey = "routingKey2";
                //    });
                //});

                cfg.ReceiveEndpoint(host, "Store",
                    ep =>
                    {
                        ep.Consumer(() => new FileReceivedConsumer());

                        // Порядок имеет значение
                        ep.UseScheduledRedelivery(ret => ret.Intervals(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60)));
                        ep.UseMessageRetry(ret => ret.Interval(2, TimeSpan.FromSeconds(5)));
                    });
                cfg.UseDelayedExchangeMessageScheduler();
            });

            bus.Start();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            bus.Stop();
        }
    }
}