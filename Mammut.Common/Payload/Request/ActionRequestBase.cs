using System;

namespace Mammut.Common.Payload.Request
{
    public class ActionRequestBase : IActionRequest
    {
        public ActionRequestBase(Guid sessionId)
        {
            this.SessionId = sessionId;
        }

        public ActionRequestBase()
        {
        }

        /// <summary>
        /// The Id of the logged in session.
        /// </summary>
        public Guid SessionId { get; set; }
    }
}
