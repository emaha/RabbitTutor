using System;
using System.Threading.Tasks;
using Common;
using MassTransit;

namespace Report
{
    internal class FileReceivedConsumer : IConsumer<IFileReceivedEvent>
    {
        public Task Consume(ConsumeContext<IFileReceivedEvent> context)
        {
            Console.WriteLine($"Updating customer: {context.Message.Message}");

            return Task.FromResult(context.Message);
        }
    }
}