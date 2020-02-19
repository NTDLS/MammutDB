using MamothDB.Server.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;

namespace MamothDB.Server.Core.Engine
{
    public class IOEngine
    {
        private ServerCore _core;
        private MemoryCache _memCache;

        public IOEngine(ServerCore core)
        {
            _core = core;

            var memCacheOptions = new MemoryCacheOptions()
            {
                SizeLimit = _core.Settings.MaxCacheSize
            };

            _memCache = new MemoryCache(memCacheOptions);
        }

        public bool DirectoryExists(MetaSession session, string path)
        {
            if (session.CurrentTransaction.DeferredIO.ContainsFilePath(path))
            {
                //The directory might not yet exist, but its in the cache.
                return true;
            }

            //TODO: need to track this as a lock:
            return Directory.Exists(path);
        }

        public bool FileExists(MetaSession session, string path)
        {
            if(session.CurrentTransaction.DeferredIO.ContainsFilePath(path))
            {
                //The file might not yet exist, but its in the cache.
                return true;
            }

            //TODO: need to track this as a lock:
            return File.Exists(path);
        }

        public void CreateDirectory(MetaSession session, string path)
        {
            session.CurrentTransaction.RecordCreateDirectory(path);
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }

        public void DeleteDirectory(MetaSession session, string path)
        {
            //RecordDeleteDirectory actually moves the directory to the undo location.
            session.CurrentTransaction.RecordDeleteDirectory(path);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// Reads JSON data from the disk WITH caching but without transactions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public T GetJson<T>(MetaSession session, string filePath)
        {
            string key = Utility.FileSystemPathToKey(filePath);

            if (_memCache.TryGetValue(key, out T value))
            {
                return value;
            }

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        /// <summary>
        /// Reads JSON data directy from the disk withoug caching or transactions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public T GetJsonDirty<T>(string filePath)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        /// <summary>
        /// Writes data in JSON format to the disk WITH caching but without transactions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="deserializedObject"></param>
        public void PutJson<T>(MetaSession session, string filePath, T deserializedObject)
        {
            string cacheKey = Utility.FileSystemPathToKey(filePath);

            bool deferDiskWrite = session.CurrentTransaction.DeferredIO.RecordDeferredDiskIO(cacheKey, filePath, deserializedObject, Constants.IOFormat.JSON);
            if (deferDiskWrite == false)
            {
                string serialized = JsonConvert.SerializeObject(deserializedObject);
                File.WriteAllText(filePath, serialized);
            }

            int objectSize = Marshal.ReadInt32(deserializedObject.GetType().TypeHandle.Value, 4);

            var options = new MemoryCacheEntryOptions()
            {
                Size = objectSize
                //TODO: Consider expirations.
            };

            _memCache.Set<T>(cacheKey, deserializedObject, options);

            session.CurrentTransaction.RecordFileWrite(filePath);
        }

        /// <summary>
        /// Writes data in JSON format to the disk without caching or transactions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="deserializedObject"></param>
        public void PutJsonDirty<T>(string filePath, T deserializedObject)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(deserializedObject));
        }

        /// <summary>
        /// Reads PBUF data directy from the disk withoug caching or transactions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public T GetPBufDirty<T>(string filePath)
        {
            using (var file = File.OpenRead(filePath))
            {
                return ProtoBuf.Serializer.Deserialize<T>(file);
            }
        }

        /// <summary>
        /// Reads PBUF data directy from the disk WITH caching but without transactions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public T GetPBuf<T>(MetaSession session, string filePath)
        {
            string key = Utility.FileSystemPathToKey(filePath);

            if (_memCache.TryGetValue<T>(key, out T value))
            {
                return value;
            }

            using (var file = File.OpenRead(filePath))
            {
                return ProtoBuf.Serializer.Deserialize<T>(file);
            }
        }

        /// <summary>
        /// Writes data in PBUF format to the disk without caching or transactions.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="deserializedObject"></param>
        public void PutPBufDirty<T>(string filePath, T deserializedObject)
        {
            using (var file = File.Create(filePath))
            {
                ProtoBuf.Serializer.Serialize(file, deserializedObject);
            }
        }

        /// <summary>
        /// Writes data in PBUF format to the disk WITH caching but without transactions.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="deserializedObject"></param>
        public void PutPBuf<T>(MetaSession session, string filePath, T deserializedObject)
        {
            string cacheKey = Utility.FileSystemPathToKey(filePath);

            bool deferDiskWrite = session.CurrentTransaction.DeferredIO.RecordDeferredDiskIO(cacheKey, filePath, deserializedObject, Constants.IOFormat.PBuf);
            if (deferDiskWrite == false)
            {
                using (var file = File.Create(filePath))
                {
                    ProtoBuf.Serializer.Serialize(file, deserializedObject);
                    file.Close();
                }
            }

            _memCache.Set<T>(cacheKey, deserializedObject);

            session.CurrentTransaction.RecordFileWrite(filePath);
        }
    }
}
