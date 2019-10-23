using Common;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Consumer
{
    internal class FileReceivedConsumer : IConsumer<IFileReceivedEvent>
    {
        public Task Consume(ConsumeContext<IFileReceivedEvent> context)
        {
            Console.WriteLine($"Updating customer: {context.Message.FileId}");

            return Task.FromResult(context.Message);
        }
    }
}