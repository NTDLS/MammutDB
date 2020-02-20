using System;
using System.Collections.Generic;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.Models
{
    public class DeferredDiskIO
    {
        private ServerCore _core;
        private Dictionary<string, DeferredDiskIOObject> _collection;

        public DeferredDiskIO(ServerCore core)
        {
            _core = core;
            _collection = new Dictionary<string, DeferredDiskIOObject>();
        }

        public bool ContainsFilePath(string filePath)
        {
            string key = Utility.FileSystemPathToKey(filePath);
            return _collection.ContainsKey(key);
        }

        /// <summary>
        /// Writes all deferred IOs to disk.
        /// </summary>
        public void Commit()
        {
            lock (this)
            {
                foreach (var deferred in _collection)
                {
                    if (deferred.Value.Reference != null)
                    {
                        if (deferred.Value.DeferredFormat == IOFormat.JSON)
                        {
                            _core.IO.PutJsonDirty(deferred.Value.DiskPath, deferred.Value.Reference);
                        }
                        else if (deferred.Value.DeferredFormat == IOFormat.PBuf)
                        {
                            _core.IO.PutPBufDirty(deferred.Value.DiskPath, deferred.Value.Reference);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }

                _collection.Clear();
            }
        }

        /// <summary>
        /// Keeps a reference to a file so that we can defer serializing and writing it to disk.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public bool RecordDeferredDiskIO(string key, string diskPath, object reference, IOFormat deferredFormat)
        {
            lock (this)
            {
                if (_collection.ContainsKey(key))
                {
                    var wrapper = _collection[key];
                    wrapper.Hits++;

                    wrapper.DiskPath = diskPath;
                    wrapper.CacheKey = key;
                    wrapper.Reference = reference;
                    wrapper.DeferredFormat = deferredFormat;
                }
                else
                {
                    var wrapper = new DeferredDiskIOObject()
                    {
                        Hits = 1,
                        DeferredFormat = deferredFormat,
                        DiskPath = diskPath,
                        CacheKey = diskPath.ToLower(),
                        Reference = reference
                    };

                    _collection.Add(key, wrapper);
                }

                return true;
            }
        }
    }
}
