using Mammut.Common.Payload.Model;
using System;

namespace Mammut.Common.Payload.Request
{
    public class ActionRequestDocument : ActionRequestBase
    {
        public ActionRequestDocument(Guid sessionId)
            : base(sessionId) { }

        public ActionRequestDocument()
            : base(Guid.Empty) { }

        public Guid Id { get; set; }
        public string Path { get; set; }
        public Document Document { get; set; }
    }
}
