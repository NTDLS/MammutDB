using Mamoth.Common.Payload.Model;
using System;

namespace Mamoth.Common.Payload.Request
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
