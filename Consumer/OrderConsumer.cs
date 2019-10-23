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

            await Console.Out.WriteLineAsync($"Обработка заказа: {context.Message}");
        }
    }
}