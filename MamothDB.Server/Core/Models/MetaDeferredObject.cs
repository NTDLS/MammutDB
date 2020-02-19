using System;
using static MamothDB.Server.Core.Constants;

namespace MamothDB.Server.Core.Models
{
    public class DeferredDiskIOObject
    {
        public string CacheKey { get; set; }
        public string DiskPath { get; set; }
        public Object Reference { get; set; }
        public long Hits { get; set; }
        public IOFormat DeferredFormat { get; set; }
    }
}
