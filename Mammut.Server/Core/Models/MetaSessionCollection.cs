using System;
using System.Collections.Generic;

namespace Mammut.Server.Core.Models
{
    /// <summary>
    /// Represents a collection of sessions in a catalog.
    /// </summary>
    [Serializable]
    public class MetaSessionCollection
    {
        public List<MetaSession> Catalog = new List<MetaSession>();

        public void Add(MetaSession session)
        {
            Catalog.Add(session);
        }

        public void Add(Mammut.Common.Payload.Model.Session session)
        {
            Catalog.Add(MetaSession.FromPayload(session));
        }

        public MetaSession GetById(Guid sessionId)
        {
            return Catalog.Find(o => o.SessionId == sessionId);
        }
    }
}
