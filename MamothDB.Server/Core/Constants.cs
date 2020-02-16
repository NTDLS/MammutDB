using System;
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
            public static string TransactionUndoCatalog = "_catalog.json";

            public static string ServerSettings = "_server.json";
            public static string TransactionDirectory = "_transaction";
            public static string SchemaDirectory = "_schema";
        }

        public enum LatchMode
        {
            Exclusive,
            Shared
        }

        public enum ObjectType
        {
            Schema,
            Document
        }

        public enum TransactionUndoAction
        {
            RestoreFile,      //Restore the specified file.
            RestoreDirectory, //Restore the specified directory.
            DeleteFile,       //Delete the specified file
            DeleteDirectory   //Delete the specified file
        }
    }
}
