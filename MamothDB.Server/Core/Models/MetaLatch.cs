using static MamothDB.Server.Core.Constants;

namespace MamothDB.Server.Core.Models
{
    /// <summary>
    /// Represents a latch.
    /// </summary>
    public class MetaLatch
    {
        public ObjectType ObjectType { get; set; }
        public string LogicalObjectPath { get; private set; }
        public string ObjectKey { get; private set; }
        public MetaLatchKeyCollection Keys { get; private set; }

        public MetaLatchKey IssueKey(MetaTransaction transaction, LatchMode mode)
        {
            var key = new MetaLatchKey(this, transaction, mode);
            Keys.Add(key);
            return key;
        }

        public void TurnInKey(MetaLatchKey key)
        {
            Keys.Remove(key);
        }

        public MetaLatch(string logicalObjectPath)
        {
            Keys = new MetaLatchKeyCollection();
            LogicalObjectPath = logicalObjectPath;
            ObjectKey = Utility.FileSystemPathToKey(logicalObjectPath);
        }
    }
}
