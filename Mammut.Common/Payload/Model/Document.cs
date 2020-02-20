using System;

namespace Mammut.Common.Payload.Model
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public Document()
        {
        }
    }
}
