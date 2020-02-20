using System;

namespace Mammut.Server.Core.State
{
    /// <summary>
    /// Represents a session.
    /// </summary>
    public class Session
    {
        public Guid SessionId { get; set; }
        public Guid LoginId { get; set; }
        public string Username { get; set; }
        public Transaction CurrentTransaction { get; set; }

        public void CommitImplicitTransaction()
        {
            if (CurrentTransaction != null)
            {
                if (CurrentTransaction.IsImplicit)
                {
                    CurrentTransaction.Commit();
                }
            }
        }

        public static Session FromPayload(Mammut.Common.Payload.Model.Session login)
        {
            return new Session
            {
                LoginId = login.LoginId,
                SessionId = login.SessionId,
                Username = login.Username
            };
        }

        public static Mammut.Common.Payload.Model.Session ToPayload(Session login)
        {
            return new Mammut.Common.Payload.Model.Session
            {
                LoginId = login.LoginId,
                SessionId = login.SessionId,
                Username = login.Username
            };
        }

    }
}
