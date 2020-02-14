﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core
{
    public class Constants
    {
        public static class Filesystem
        {
            public static string SchemaCatalog = "_schemas.json";
            public static string DocumentCatalog = "_documents.json";
            public static string LoginCatalog = "_logins.json";

            public static string ServerSettings = "_server.json";
            public static string TransactionDirectory = "_transaction";
            public static string SchemaDirectory = "_schema";
        }
    }
}