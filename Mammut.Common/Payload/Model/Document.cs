using System;

namespace Mammut.Common.Payload.Model
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Content { get; set; }

        public Document()
        {
        }
    }
}
