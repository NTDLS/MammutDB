using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.State
{
    /// <summary>
    /// Represents a latch.
    /// </summary>
    public class Latch
    {
        public ObjectType ObjectType { get; set; }
        public string LogicalObjectPath { get; private set; }
        public string ObjectKey { get; private set; }
        public LatchKeyCollection Keys { get; private set; }

        public LatchKey IssueKey(Transaction transaction, LatchMode mode)
        {
            var key = new LatchKey(this, transaction, mode);
            Keys.Add(key);
            return key;
        }

        public void TurnInKey(LatchKey key)
        {
            Keys.Remove(key);
        }

        public Latch(string logicalObjectPath)
        {
            Keys = new LatchKeyCollection();
            LogicalObjectPath = logicalObjectPath;
            ObjectKey = Utility.FileSystemPathToKey(logicalObjectPath);
        }
    }
}
