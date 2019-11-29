using Common;
using MassTransit;
using System;
using System.Threading.Tasks;
using EasyNetQ;

namespace Consumer
{
    internal class FileReceivedConsumer : IConsumer<IFileReceivedEvent>
    {
        private readonly int _id;

        public FileReceivedConsumer(int id = 0)
        {
            _id = id;
        }

        public Task Consume(ConsumeContext<IFileReceivedEvent> context)
        {
            Console.WriteLine("TIME: " + DateTime.Now.ToString("O"));
            Console.WriteLine($"ID: {_id}. Updating customer: {context.Message.Message}");

            return Task.FromResult(context.Message);
        }
    }
}