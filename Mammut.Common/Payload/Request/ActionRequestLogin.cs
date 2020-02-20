using Mammut.Common.Payload.Model;
using System;

namespace Mammut.Common.Payload.Request
{
    public class ActionRequestLogin : ActionRequestBase
    {
        public ActionRequestLogin(Guid sessionId)
            : base(sessionId) { }

        public ActionRequestLogin()
            : base(Guid.Empty) { }

        public Login Login { get; set; }
    }
}
