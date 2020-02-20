using System;

namespace Mammut.Server.Core.Models.Persist
{
    /// <summary>
    /// Represents a single document.
    /// </summary>
    public class MetaDocument
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public static MetaDocument FromPayload(Mammut.Common.Payload.Model.Document document)
        {
            return new MetaDocument
            {
                Id = document.Id,
                Text = document.Text
            };
        }

        public static Mammut.Common.Payload.Model.Document ToPayload(MetaDocument document)
        {
            return new Mammut.Common.Payload.Model.Document
            {
                Id = document.Id,
                Text = document.Text
            };
        }
    }
}
