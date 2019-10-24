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
                var host = cfg.Host(new Uri("rabbitmq://192.168.0.192"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(host, "Store",
                    ep =>
                    {
                        ep.Consumer(() => new FileReceivedConsumer());

                        // Порядок имеет значение
                        //ep.UseScheduledRedelivery(ret => ret.Intervals(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60)));
                        //ep.UseMessageRetry(ret => ret.Interval(2, TimeSpan.FromSeconds(5)));
                    });
                //cfg.UseDelayedExchangeMessageScheduler();
            });

            bus.Start();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            bus.Stop();

            #region old

            //MASSTRANSIT COMMAND
            //var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            //{
            //    var host = cfg.Host(new Uri("rabbitmq://192.168.0.192"), h =>
            //    {
            //        h.Username("guest");
            //        h.Password("guest");
            //    });
            //    cfg.UseDelayedExchangeMessageScheduler();

            //    cfg.ReceiveEndpoint(host, "hello",
            //        ep =>
            //        {
            //            // Порядок имеет значение
            //            ep.UseScheduledRedelivery(ret => ret.Intervals(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60)));
            //            ep.UseMessageRetry(ret => ret.Interval(2, TimeSpan.FromSeconds(5)));

            //            ep.Consumer<OrderConsumer>();
            //        });
            //});

            //bus.Start();

            //Console.WriteLine("Press any key to exit");
            //Console.ReadKey();

            //bus.Stop();

            // EasyNetQ
            //using (var bus = RabbitHutch.CreateBus("host=192.168.0.192"))
            //{
            //    bus.Subscribe<Order>("my_subscription_id", order =>
            //   {
            //       Console.Write($"Working... id={order.Id}\t");
            //       Thread.Sleep(1000);
            //       Console.WriteLine(order.ToString());
            //   });
            //    Console.ReadLine();
            //}

            // RabbitMQ.Client
            //var factory = new ConnectionFactory() { HostName = "192.168.0.192" };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    // set the heartbeat timeout to 200 seconds
            //    factory.RequestedHeartbeat = 2;

            //    channel.QueueDeclare(queue: "hello",
            //        durable: false,
            //        exclusive: false,
            //        autoDelete: false,
            //        arguments: null);

            //    var consumer = new EventingBasicConsumer(channel);
            //    consumer.Received += (model, ea) =>
            //    {
            //        var body = ea.Body;
            //        MemoryStream ms = new MemoryStream(body);
            //        DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Order));
            //        var order = jsonFormatter.ReadObject(ms);
            //        Thread.Sleep(2000);

            //        if (r.Next(2) % 2 == 0)
            //        {
            //            Console.Write("+");
            //            channel.BasicAck(ea.DeliveryTag, false);
            //        }
            //        else
            //        {
            //            Console.Write("-");
            //            channel.BasicNack(ea.DeliveryTag, false, true);
            //        }

            //        Console.WriteLine($" : {DateTime.Now} : Received data: {order.ToString()}");
            //    };

            //    channel.BasicConsume(queue: "hello",
            //        autoAck: false,
            //        consumer: consumer);

            //Console.WriteLine(" Press [enter] to exit.");
            //Console.ReadLine();

            #endregion old
        }
    }
}