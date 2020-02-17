using System.IO;

namespace MamothDB.Server.Core
{
    public static class Utility
    {
        public static string FileSystemPathToKey(string path)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                path = path.Replace(c.ToString(), "_");
            }

            return path.ToLower();
        }
    }
}
