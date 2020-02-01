using Mamoth.Common;
using Mamoth.Common.Payload.Model;
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
            var schemaParts = MamothUtility.SplitSchema(schema);

            return Guid.Empty;
        }

    }
}