using System;

namespace Mammut.Common.Payload.Model
{
    public class User
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modfied { get; set; }
        public string PasswordHash { get; set; }

        public User()
        {
        }

        public User(string name)
        {
            this.Name = name;
        }

        public User(string name, string passwordHash)
        {
            this.Name = name;
            this.PasswordHash = passwordHash;
        }
    }
}
