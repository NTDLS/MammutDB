using System;
using System.Collections.Generic;

namespace Mammut.Server.Core.Models.Persist
{
    /// <summary>
    /// Represents a collection of documents in a catalog.
    /// </summary>
    public class MetaDocumentCollection
    {
        public List<Guid> Catalog = new List<Guid>();

        public void Add(MetaDocument document)
        {
            Catalog.Add(document.Id);
        }

        public void Add(Mammut.Common.Payload.Model.Document document)
        {
            Catalog.Add(document.Id);
        }
    }
}