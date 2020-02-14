using Mamoth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Models.Persist
{
    /// <summary>
    /// Represents a user/login.
    /// </summary>
    public class MetaLogin
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public MetaLogin()
        {
        }

        public MetaLogin(string username, string passwordHash)
        {
            Id = Guid.NewGuid();
            Username = username;
            PasswordHash = passwordHash;
        }

        public MetaLogin(string username)
        {
            Id = Guid.NewGuid();
            Username = username;
        }

        public void SetPassword(string plainTextPassword)
        {
            PasswordHash = MamothUtility.HashPassword(plainTextPassword);
        }

        public static MetaLogin FromPayload(Mamoth.Common.Payload.Model.Login login)
        {
            return new MetaLogin
            {
                Id = login.Id,
                Username = login.Username,
                PasswordHash = login.PasswordHash
            };
        }

        public static Mamoth.Common.Payload.Model.Login ToPayload(MetaLogin login)
        {
            return new Mamoth.Common.Payload.Model.Login
            {
                Id = login.Id,
                Username = login.Username,
                PasswordHash = login.PasswordHash
            };
        }
    }
}
