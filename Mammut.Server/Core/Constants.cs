namespace Mammut.Server.Core
{
    public class Constants
    {
        public static class Filesystem
        {
            public static string SchemaCatalog = "_schemas.json";
            public static string DocumentCatalog = "_documents.json";
            public static string LoginCatalog = "_logins.json";
            public static string TransactionUndoCatalog = "_catalog.json";
            public static string DocumentFileExtention = ".json";
            public static string TxUnDoExtension = ".tx";

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

        public enum IOFormat
        {
            Raw,
            JSON,
            PBuf
        }

        public enum TransactionUndoAction
        {
            RestoreFile,      //Restore the specified file.
            RestoreDirectory, //Restore the specified directory.
            DeleteFile,       //Delete the specified file
            DeleteDirectory   //Delete the specified file
        }

        public enum QueryType
        {
            Select,
            Insert,
            Update,
            Delete
        }

        public enum ConditionType
        {
            None, //This means that this is the first expression in the tree.
            And,
            Or
        }

        public enum ConditionValueType
        {
            /// <summary>
            /// Denotes whether the value is a constant such as a string ot numeric value.
            /// </summary>
            Constant,
            /// <summary>
            /// Denotes whether the value represents an attribute name contained in a document.
            /// </summary>
            AttributeName
        }

        public enum ConditionQualifier
        {
            Equals,
            Like,
            NotEquals,
            NotLike,
            LessThan,
            GreaterThan,
            LessThanOrEqual,
            GreaterThanOrEqual
        }
    }
}
