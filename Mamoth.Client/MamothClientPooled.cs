using System;

namespace Mamoth.Client
{
    public class MamothClientPooled : MamothClientBase, IDisposable
    {
        public MamothClientPooled(string baseAddress, TimeSpan commandTimeout)
            : base(baseAddress, commandTimeout)
        {
        }

        public MamothClientPooled(string baseAddress)
            : base(baseAddress)
        {
        }

        public MamothClientPooled(string baseAddress, string username, string password)
            : base(baseAddress, username, password)
        {
        }

        public MamothClientPooled(string baseAddress, TimeSpan commandTimeout, string username, string password)
            : base(baseAddress, commandTimeout, username, password)
        {
        }
    }
}
