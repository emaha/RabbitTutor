using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using MassTransit;

namespace Consumer
{
    public class OrderConsumer : IConsumer<Order>
    {
        public async Task Consume(ConsumeContext<Order> context)
        {
            Console.WriteLine("processing");
            Thread.Sleep(5000);

            if (new Random().Next(2) % 2 == 0)
            {
                throw new Exception("Very bad things happened");
            }

            await Console.Out.WriteLineAsync($"Обработка заказа: {context.Message}");
        }
    }
}