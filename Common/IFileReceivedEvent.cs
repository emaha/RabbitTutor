using System;

namespace Common
{
    public interface IFileReceivedEvent
    {
        string Message { get; }
    }
}