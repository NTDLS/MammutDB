using Mammut.Common;
using System;

namespace Mammut.Server.Core.Models.Persist
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
            PasswordHash = MammutUtility.HashPassword(plainTextPassword);
        }

        public static MetaLogin FromPayload(Mammut.Common.Payload.Model.Login login)
        {
            return new MetaLogin
            {
                Id = login.Id,
                Username = login.Username,
                PasswordHash = login.PasswordHash
            };
        }

        public static Mammut.Common.Payload.Model.Login ToPayload(MetaLogin login)
        {
            return new Mammut.Common.Payload.Model.Login
            {
                Id = login.Id,
                Username = login.Username,
                PasswordHash = login.PasswordHash
            };
        }
    }
}
