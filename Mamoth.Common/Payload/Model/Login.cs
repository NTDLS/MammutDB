using System;

namespace Mamoth.Common.Payload.Model
{
    public class Login
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public Login()
        {
        }

        public Login(string username)
        {
            this.Username = username;
        }

        public Login(string username, string plainTextPassword)
        {
            this.Username = username;
            this.PasswordHash = MamothUtility.HashPassword(plainTextPassword);
        }
    }
}
