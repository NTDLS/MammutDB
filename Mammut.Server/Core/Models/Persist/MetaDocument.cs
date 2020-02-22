using System;

namespace Mammut.Server.Core.Models.Persist
{
    /// <summary>
    /// Represents a single document.
    /// </summary>
    public class MetaDocument
    {
        public Guid Id { get; set; }
        public string Content { get; set; }

        public static MetaDocument FromPayload(Mammut.Common.Payload.Model.Document document)
        {
            return new MetaDocument
            {
                Id = document.Id,
                Content = document.Content
            };
        }

        public static Mammut.Common.Payload.Model.Document ToPayload(MetaDocument document)
        {
            return new Mammut.Common.Payload.Model.Document
            {
                Id = document.Id,
                Content = document.Content
            };
        }
    }
}
