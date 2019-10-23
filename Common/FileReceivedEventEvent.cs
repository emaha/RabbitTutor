using System;

namespace Common
{
    public class FileReceivedEventEvent : IFileReceivedEvent
    {
        public Guid FileId { get; set; }
        public DateTime Timestamp { get; set; }
        public Uri Location { get; set; }
    }
}