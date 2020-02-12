using System;
using System.Collections.Generic;
using System.Text;

namespace Mamoth.Client
{
    public class MamothConnection : IDisposable
    {
        public MamothClientPooled Client { get; set; }
        public MamothConnectionPool _pool { get; set; }

        public MamothConnection(MamothConnectionPool pool, MamothClientPooled client)
        {
            Client = client;
            _pool = pool;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // get rid of managed resources:
                _pool.ReleaseConnection(this);
            }
            // get rid of unmanaged resources:
        }
    }
}
