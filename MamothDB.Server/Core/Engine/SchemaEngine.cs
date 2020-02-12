using Mamoth.Common;
using Mamoth.Common.Payload.Model;
using Mamoth.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Engine
{
    public class SchemaEngine
    {
        private ServerCore _core;

        public SchemaEngine(ServerCore core)
        {
            _core = core;
        }

        /// <summary>
        /// Creates a new single schema and returns its name and ID.
        /// </summary>
        /// <param name="schemaPath"></param>
        /// <returns></returns>
        public Guid Create(string schema)
        {
            var schemaParts = SplitSchema(schema);

            return Guid.Empty;
        }

        /// <summary>
        /// Splits a full schema into its path and name parts.
        /// </summary>
        /// <returns></returns>
        public SchemaInfo SplitSchema(string schema)
        {
            schema = schema.Trim(new char[] { ':' }).Replace("::", ":");

            var parts = new SchemaInfo();

            int lastDelimiterIndex = schema.LastIndexOf(":");

            if (lastDelimiterIndex > 0)
            {
                parts.LogicalParent = schema.Substring(0, lastDelimiterIndex);
                parts.Name = schema.Substring(lastDelimiterIndex + 1);
                parts.FullLogicalPath = $"{parts.LogicalParent}:{parts.Name}".Replace("::", ":");
                parts.ParentDiskPath = System.IO.Path.Join(_core.Settings.SchemaPath, parts.LogicalParent.Replace(':', '\\'));
                parts.FullDiskPath = System.IO.Path.Join(parts.ParentDiskPath, parts.Name);
            }
            else {
                parts.Name = schema;
                parts.LogicalParent = "";
                parts.FullLogicalPath = schema;
                parts.ParentDiskPath = _core.Settings.SchemaPath;
                parts.FullDiskPath = System.IO.Path.Join(_core.Settings.SchemaPath, parts.Name);
            }

            return parts;
        }

    }
}