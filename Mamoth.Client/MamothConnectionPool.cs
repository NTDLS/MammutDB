using System;
using System.Collections.Generic;

namespace Mamoth.Client
{
    /*
    using (var pool = new MamothConnectionPool("https://localhost:5001", "root", ""))
    {
        using (var connection = pool.GetConnection())
        {
            connection.Client.Schema.Create("AR");
            connection.Client.Schema.Create("AR:Sales");
            connection.Client.Schema.Create("AR:Sales:Orders");
            connection.Client.Schema.Create("AR:Sales:People");
            connection.Client.Schema.Create("AR:Sales:People:Terminated");
            connection.Client.Schema.Create("AR:Customers");
            connection.Client.Schema.Create("AR:Customers:Prospects");
            connection.Client.Schema.Create("AR:Customers:Contracts");
        }
    }
    */
    public class MamothConnectionPool: IDisposable
    {
        private readonly string _baseAddress;
        private readonly TimeSpan _commandTimeout;
        private readonly string _username;
        private readonly string _password;
        private readonly int _maxConnections = 100;

        private List<MamothClientPooled> _pool = new List<MamothClientPooled>();
        private HashSet<MamothClientPooled> _inUse = new HashSet<MamothClientPooled>();

        public MamothConnectionPool(string baseAddress, string username, string password)
        {
            _baseAddress = baseAddress;
            _username = username;
            _password = password;
            _commandTimeout = new TimeSpan(0, 0, 30);
        }

        public MamothConnectionPool(string baseAddress, string username, string password, int maxConnections)
        {
            _baseAddress = baseAddress;
            _username = username;
            _password = password;
            _commandTimeout = new TimeSpan(0, 0, 30);
            _maxConnections = maxConnections;
        }

        public MamothConnectionPool(string baseAddress, string username, string password, int maxConnections, TimeSpan commandTimeout)
        {
            _baseAddress = baseAddress;
            _username = username;
            _password = password;
            _commandTimeout = commandTimeout;
            _maxConnections = maxConnections;
        }

        public MamothConnectionPool(string baseAddress, string username, string password, TimeSpan commandTimeout)
        {
            _baseAddress = baseAddress;
            _username = username;
            _password = password;
            _commandTimeout = commandTimeout;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public MamothConnection GetConnection()
        {
            foreach (var client in _pool)
            {
                if (_inUse.Contains(client) == false)
                {
                    _inUse.Add(client);
                    return new MamothConnection(this, client);
                }
            }

            if (_pool.Count >= _maxConnections)
            {
                throw new Exception("The connnection pool has reached its maximum size.");
            }

            var newClient = new MamothClientPooled(_baseAddress, _commandTimeout, _username, _password);

            _inUse.Add(newClient);
            _pool.Add(newClient);

            return new MamothConnection(this, newClient);
        }

        public void ReleaseConnection(MamothConnection connection)
        {
            //TODO: We really need to reset the connection somehow. Like if it has an open tran, that could suck.
            if (_inUse.Contains(connection.Client))
            {
                _inUse.Remove(connection.Client);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // get rid of managed resources:
                foreach (var client in _pool)
                {
                    client.Dispose();
                }

                _pool.Clear();
                _inUse.Clear();
            }
            // get rid of unmanaged resources:
        }
    }
}
