using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Models
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

        public void Add(Mamoth.Common.Payload.Model.Session session)
        {
            Catalog.Add(MetaSession.FromPayload(session));
        }

        public MetaSession GetById(Guid sessionId)
        {
            return Catalog.Find(o => o.SessionId == sessionId);
        }
    }
}
