using System;

namespace Common
{
    public class FileReceivedEventEvent : IFileReceivedEvent
    {
        public string Message { get; set; }
    }
}