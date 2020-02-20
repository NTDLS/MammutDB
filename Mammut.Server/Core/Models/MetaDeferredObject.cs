using System;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.Models
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
