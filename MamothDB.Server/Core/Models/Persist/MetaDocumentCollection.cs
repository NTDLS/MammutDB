﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
