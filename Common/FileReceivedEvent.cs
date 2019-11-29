using System;

namespace Common
{
    public class FileReceivedEvent : IFileReceivedEvent
    {
        public string Message { get; set; }
    }
}