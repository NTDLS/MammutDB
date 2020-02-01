using Mamoth.Common.Types;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mamoth.Common
{
    public static class MamothUtility
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            return (new StackTrace()).GetFrame(1).GetMethod().Name;
        }

        public static string HashPassword(string password)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        /// <summary>
        /// Splits a full schema into its path and name parts.
        /// </summary>
        /// <returns></returns>
        public static SchemaParts SplitSchema(string schema)
        {
            schema  = schema.Trim(new char[] { ':' });

            var parts = new SchemaParts()
            {
                Path = string.Empty,
                Name = schema
            };

            int lastDelimiterIndex = schema.LastIndexOf(":");

            if (lastDelimiterIndex > 0)
            {
                parts.Path = schema.Substring(0, lastDelimiterIndex);
                parts.Name = schema.Substring(lastDelimiterIndex + 1);
            }

            return parts;
        }

    }
}
