using MamothDB.Server.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            //TODO: need to track this as a lock:
            return Directory.Exists(path);
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
            string key = Utility.FileSystemPathToKey(filePath);

            string serialized = JsonConvert.SerializeObject(deserializedObject);

            var options = new MemoryCacheEntryOptions()
            {
                Size = serialized.Length
                //TODO: Consider expirations.
            };

            _memCache.Set<T>(key, deserializedObject, options);

            session.CurrentTransaction.RecordFileWrite(filePath);

            File.WriteAllText(filePath, serialized);
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
            string key = Utility.FileSystemPathToKey(filePath);

            long serializedLength = 0;

            session.CurrentTransaction.RecordFileWrite(filePath);

            using (var file = File.Create(filePath))
            {
                ProtoBuf.Serializer.Serialize(file, deserializedObject);
                serializedLength = file.Length;
                file.Close();
            }

            var options = new MemoryCacheEntryOptions()
            {
                Size = serializedLength
                //TODO: Consider expirations.
            };

            _memCache.Set<T>(key, deserializedObject, options);
        }
    }
}
