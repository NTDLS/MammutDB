using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mammut.Server.Core.State
{
    public static class CriticalSections
    {
        public static object AcquireLock = new object();
    }
}
