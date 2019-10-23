using System;

namespace Common
{
    public interface IFileReceivedEvent
    {
        Guid FileId { get; }
        DateTime Timestamp { get; }
        Uri Location { get; }
    }
}