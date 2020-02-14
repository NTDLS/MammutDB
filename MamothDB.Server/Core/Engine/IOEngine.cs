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

        private string FileSystemPathToKey(string path)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                path = path.Replace(c.ToString(), "_");
            }

            return path.ToLower();
        }

        /// <summary>
        /// Reads JSON data from the disk WITH caching but without transactions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public T GetJson<T>(MetaSession session, string filePath)
        {
            string key = FileSystemPathToKey(filePath);

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
            string key = FileSystemPathToKey(filePath);

            string serialized = JsonConvert.SerializeObject(deserializedObject);

            var options = new MemoryCacheEntryOptions()
            {
                Size = serialized.Length
                //TODO: Consider expirations.
            };

            _memCache.Set<T>(key, deserializedObject, options);

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
            string key = FileSystemPathToKey(filePath);

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
            string key = FileSystemPathToKey(filePath);

            long serializedLength = 0;

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
