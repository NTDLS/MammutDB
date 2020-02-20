using System;
using System.Collections.Generic;

namespace Mammut.Server.Core.State
{
    /// <summary>
    /// Represents a collection of sessions in a catalog.
    /// </summary>
    [Serializable]
    public class SessionCollection
    {
        public List<Session> Catalog = new List<Session>();

        public void Add(Session session)
        {
            Catalog.Add(session);
        }

        public void Add(Mammut.Common.Payload.Model.Session session)
        {
            Catalog.Add(Session.FromPayload(session));
        }

        public Session GetById(Guid sessionId)
        {
            return Catalog.Find(o => o.SessionId == sessionId);
        }
    }
}
