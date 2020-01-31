using System;

namespace Mamoth.Common.Payload.Request
{
    public class ActionRequestGenericObject : ActionRequestBase
    {
        public ActionRequestGenericObject(Guid sessionId)
            : base(sessionId)
        {
            this.SessionId = sessionId;
        }

        /// <summary>
        /// The containing schema.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// The name of the object
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// The Id of the object
        /// </summary>
        public Guid ObjectId { get; set; }
    }
}
