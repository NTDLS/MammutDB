using Mamoth.Common.Payload.Model;
using System;

namespace Mamoth.Common.Payload.Request
{
    public class ActionRequestSchema : ActionRequestBase
    {
        public ActionRequestSchema(Guid sessionId)
            : base(sessionId) { }

        public ActionRequestSchema()
            : base(Guid.Empty) { }

        public string Path { get; set; }
    }
}
