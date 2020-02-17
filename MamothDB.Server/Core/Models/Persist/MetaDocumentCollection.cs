using System.Collections.Generic;

namespace MamothDB.Server.Core.Models.Persist
{
    /// <summary>
    /// Represents a collection of documents in a catalog.
    /// </summary>
    public class MetaDocumentCollection
    {
        List<MetaDocument> Catalog = new List<MetaDocument>();
    }
}
